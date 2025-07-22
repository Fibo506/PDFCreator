using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Query;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Core.Workflow.Exceptions;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.IO;
using pdfforge.PDFCreator.Utilities.Tokens;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Workflow.Output;

public interface IOutputFileMover
{
    /// <summary>
    ///     Renames and moves all files from TempOutputFiles to their destination according to
    ///     the FilenameTemplate and stores them in the OutputFiles list.
    ///     For multiple files the FilenameTemplate gets an appendix.
    /// </summary>
    Task MoveOutputFiles(Job job);
}

public abstract class OutputFileMoverBase : IOutputFileMover
{
    private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);
    protected readonly Logger Logger = LogManager.GetCurrentClassLogger();

    protected IUniqueFilenameFactory UniqueFilenameFactory { get; }
    protected IFile File { get; }
    protected IPathUtil PathUtil { get; }
    protected IDirectoryHelper DirectoryHelper { get; }
    private readonly IPdfProcessor _pdfProcessor;
    private readonly IFileIndexHelper _fileIndexHelper;

    protected OutputFileMoverBase(IUniqueFilenameFactory uniqueFilenameFactory, IFile file, IPathUtil pathUtil,
        IDirectoryHelper directoryHelper, IPdfProcessor pdfProcessor, IFileIndexHelper fileIndexHelper)
    {
        UniqueFilenameFactory = uniqueFilenameFactory;
        File = file;
        PathUtil = pathUtil;
        DirectoryHelper = directoryHelper;
        _pdfProcessor = pdfProcessor;
        _fileIndexHelper = fileIndexHelper;
    }

    protected abstract Task<QueryResult<string>> HandleInvalidRootedPath(string filename, OutputFormat outputFormat);

    protected abstract Task<QueryResult<string>> HandleFirstFileFailed(string filename, OutputFormat outputFormat);

    protected abstract HandleCopyErrorResult QueryHandleCopyError(int fileNumber);

    protected abstract bool ShouldApplyUniqueFilename(Job job);

    protected abstract bool ShouldApplyMerger(Job job);

    /// <summary>
    ///     Renames and moves all files from TempOutputFiles to their destination according to
    ///     the FilenameTemplate and stores them in the OutputFiles list.
    ///     For multiple files the FilenameTemplate gets an appendix.
    /// </summary>
    public async Task MoveOutputFiles(Job job)
    {
        Logger.Trace("Moving output files to final location");

        var replacedFileIndexOutFileTemplate = _fileIndexHelper.ReplaceFileIndex(job.OutputFileTemplate, job.TempOutputFiles.Count);

        if (!PathUtil.IsValidRootedPath(replacedFileIndexOutFileTemplate))
        {
            var result = await HandleInvalidRootedPath(job.OutputFileTemplate, job.Profile.OutputFormat);
            if (result.Success == false)
            {
                throw new AbortWorkflowException("User cancelled retyping invalid rooted path.");
            }
            job.OutputFileTemplate = result.Data;
        }

        var outputDirectory = PathSafe.GetDirectoryName(job.OutputFileTemplate);

        DirectoryHelper.CreateDirectory(outputDirectory);

        //Ensure the first file is the first in TempOutputFiles
        job.TempOutputFiles = job.TempOutputFiles.OrderBy(x => x).ToList();

        int fileNumber = 0;
        foreach (var tempOutputFile in job.TempOutputFiles)
        {
            fileNumber++;

            var extension = PathSafe.GetExtension(tempOutputFile);
            job.OutputFileTemplate = PathSafe.ChangeExtension(job.OutputFileTemplate, extension);
            var fileIndex = DetermineFileIndex(job, tempOutputFile);
            var currentOutputFile = _fileIndexHelper.ReplaceFileIndex(job.OutputFileTemplate, job.TempOutputFiles.Count, fileIndex);

            await SemaphoreSlim.WaitAsync();

            try
            {
                var uniqueFilename = UniqueFilenameFactory.Build(currentOutputFile);

                bool success;
                if (!File.Exists(currentOutputFile))
                {
                    success = CopyFile(tempOutputFile, currentOutputFile);
                }
                else if (ShouldApplyMerger(job))
                {
                    success = AppendFile(tempOutputFile, currentOutputFile, job);
                }
                else
                {
                    if (ShouldApplyUniqueFilename(job))
                    {
                        currentOutputFile = EnsureUniqueFilename(uniqueFilename);
                    }

                    success = CopyFile(tempOutputFile, currentOutputFile);
                }

                if (!success)
                {
                    var action = QueryHandleCopyError(fileNumber);

                    switch (action)
                    {
                        case HandleCopyErrorResult.Requery:
                            currentOutputFile = await RequeryFilename(job, tempOutputFile, fileIndex, extension);
                            break;

                        default:
                            currentOutputFile = EnsureUniqueFilename(uniqueFilename);

                            if (!CopyFile(tempOutputFile, currentOutputFile))
                            {
                                throw new ProcessingException("Error while copying to target file in second attempt. Process gets canceled.", ErrorCode.Conversion_ErrorWhileCopyingOutputFile);
                            }

                            break;
                    }
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }

            DeleteFile(tempOutputFile);
            job.OutputFiles.Add(currentOutputFile);
        }
        job.OutputFiles = job.OutputFiles.OrderBy(x => x).ToList();
    }

    private int DetermineFileIndex(Job job, string tempOutputFile)
    {
        var tempFileBase = PathSafe.GetFileNameWithoutExtension(tempOutputFile) ?? "output";
        var num = tempFileBase.Replace(job.JobTempFileName, "");
        if (int.TryParse(num, out var numValue))
            return numValue;

        return 1;
    }

    private async Task<string> RequeryFilename(Job job, string tempOutputFile, int fileIndex, string extension)
    {
        while (true)
        {
            var result = await HandleFirstFileFailed(job.OutputFileTemplate, job.Profile.OutputFormat);

            if (result.Success == false)
            {
                throw new AbortWorkflowException("User cancelled during retype filename");
            }

            job.OutputFileTemplate = PathSafe.ChangeExtension(result.Data, extension);
            var currentOutputFile = _fileIndexHelper.ReplaceFileIndex(job.OutputFileTemplate, job.TempOutputFiles.Count, fileIndex);

            if (CopyFile(tempOutputFile, currentOutputFile))
                return currentOutputFile;
        }
    }

    /// <summary>
    ///     Ensure unique filename.
    /// </summary>
    /// <param name="uniqueFilename">The UniqueFilename object that should be used</param>
    /// <returns>unique outputfilename</returns>
    private string EnsureUniqueFilename(IUniquePath uniqueFilename)
    {
        try
        {
            Logger.Debug("Ensuring unique filename for: " + uniqueFilename.OriginalFilename);
            var newFilename = uniqueFilename.CreateUniqueFileName();
            Logger.Debug("Unique filename result: " + newFilename);
            return newFilename;
        }
        catch (PathTooLongException ex)
        {
            throw new ProcessingException(ex.Message, ErrorCode.Conversion_PathTooLong);
        }
    }

    private void DeleteFile(string tempfile)
    {
        try
        {
            File.Delete(tempfile);
        }
        catch (IOException)
        {
            Logger.Warn("Could not delete temporary file \"" + tempfile + "\"");
        }
    }

    /// <summary>
    ///     Copy file with logging and catching of ioException
    /// </summary>
    /// <returns>true if successful</returns>
    private bool CopyFile(string tempFile, string outputFile)
    {
        try
        {
            File.Copy(tempFile, outputFile, true);
            Logger.Debug("Copied output file \"{0}\" \r\nto \"{1}\"", tempFile, outputFile);
            return true;
        }
        catch (Exception ex)
        {
            Logger.Warn("Error while copying to target file.\r\nfrom\"{0}\" \r\nto \"{1}\"\r\n{2}", tempFile, outputFile, ex.Message);
        }
        return false;
    }

    private bool AppendFile(string tempFile, string outputFile, Job job)
    {
        try
        {
            _pdfProcessor.MergePDFs(outputFile, tempFile, job.Passwords.PdfOwnerPassword);
            Logger.Debug("Append output file \"{0}\" \r\ninto \"{1}\"", tempFile, outputFile);
        }
        catch (Exception ex)
        {
            Logger.Warn("Error while append into target file.\r\nfrom\"{0}\" \r\nto \"{1}\"\r\n{2}", tempFile, outputFile, ex.Message);
            return false;
        }

        //Redo signing or encryption if required
        if (job.Profile.PdfSettings.Security.Enabled || job.Profile.PdfSettings.Signature.Enabled)
        {
            try
            {
                //copy output file to intermediate file for reusing encryption/signing logic and remain in temp folder
                CopyFile(outputFile, job.IntermediatePdfFile);
                _pdfProcessor.SignEncryptConvertPdfAAndWriteFile(job);
                CopyFile(job.IntermediatePdfFile, outputFile);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "Error while re-sign/encrypt merged output file");
                return false;
            }
        }

        return true;
    }
}

public enum HandleCopyErrorResult
{
    EnsureUniqueFilename,
    Requery
}
