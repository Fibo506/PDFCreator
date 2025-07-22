using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Query;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.IO;
using pdfforge.PDFCreator.Utilities.Tokens;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Workflow.Output;

public class AutosaveOutputFileMover : OutputFileMoverBase
{
    public AutosaveOutputFileMover(IUniqueFilenameFactory uniqueFilenameFactory, IFile file, IPathUtil pathUtil, IDirectoryHelper directoryHelper,
        IPdfProcessor pdfProcessor, IFileIndexHelper fileIndexHelper)
        : base(uniqueFilenameFactory, file, pathUtil, directoryHelper, pdfProcessor, fileIndexHelper)
    { }

    protected override Task<QueryResult<string>> HandleInvalidRootedPath(string filename, OutputFormat outputFormat)
    {
        return Task.FromResult(new QueryResult<string>(false, null));
    }

    protected override Task<QueryResult<string>> HandleFirstFileFailed(string filename, OutputFormat outputFormat)
    {
        return Task.FromResult(new QueryResult<string>(false, null));
    }

    protected override HandleCopyErrorResult QueryHandleCopyError(int fileNumber)
    {
        return HandleCopyErrorResult.EnsureUniqueFilename;
    }

    protected override bool ShouldApplyUniqueFilename(Job job)
    {
        if (job.Profile.AutoSave.ExistingFileBehaviour == AutoSaveExistingFileBehaviour.Merge && !job.Profile.OutputFormat.IsPdf())
            return true;

        return job.Profile.AutoSave.ExistingFileBehaviour == AutoSaveExistingFileBehaviour.EnsureUniqueFilenames;
    }

    protected override bool ShouldApplyMerger(Job job)
    {
        if (!job.Profile.OutputFormat.IsPdf())
            return false;

        return job.Profile.AutoSave.ExistingFileBehaviour == AutoSaveExistingFileBehaviour.Merge;
    }
}
