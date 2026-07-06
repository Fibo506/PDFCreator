using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.DirectConversion;
using pdfforge.PDFCreator.Core.Printing.Printing;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Controller;

public class FileConversionHelper:IFileConversionHelper
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IDirectConversion _directConversion;
    private readonly IFile _file;
    private readonly IDirectory _directory;
    private readonly IStoredParametersManager _storedParametersManager;
    private readonly IPrintFileHelper _printFileHelper;

    public FileConversionHelper(IDirectConversion directConversion,
        IPrintFileHelper printFileHelper,
        IFile file,
        IDirectory directory,
        IStoredParametersManager storedParametersManager)
    {
        _directConversion = directConversion;
        _printFileHelper = printFileHelper;
        _file = file;
        _directory = directory;
        _storedParametersManager = storedParametersManager;
    }

    public void HandleFileListWithoutTooManyFilesWarning(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters)
    {
        appStartParameters.Silent = true;
        HandleFileList(droppedFiles, appStartParameters);
    }

    /// <summary>
    ///     Removes invalid files and launches print jobs for the files that needs to be printed.
    ///     If successful, the direct convertable files are added to the current JobInfoQueue.
    /// </summary>
    public void HandleFileList(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters)
    {
        if (droppedFiles == null)
            return;

        Logger.Debug("Launched Drag & Drop");
        var existingFiles = GetExistingFiles(droppedFiles);

        HandleFiles(existingFiles, appStartParameters);
    }
    public Task<JobInfo> GetJobInfoForPreviewMerge(IEnumerable<string> droppedFiles)
    {
        return null;
    }

    private List<string> GetExistingFiles(IEnumerable<string> droppedFiles)
    {
        var existingFiles = new List<string>();
        foreach (var droppedFile in droppedFiles)
        {
            if (_file.Exists(droppedFile))
            {
                existingFiles.Add(droppedFile);
            }
            else if (_directory.Exists(droppedFile))
            {
                var directoryFiles = _directory.GetFiles(droppedFile);
                foreach (var file in directoryFiles)
                {
                    existingFiles.Add(file);
                }
            }
            else
            {
                Logger.Warn("The file or directory " + droppedFile + " does not exist.");
            }
        }

        return existingFiles;
    }

    /// <summary>
    ///     Launches a print job for all dropped files that can be printed.
    ///     Return false if cancelled because of unprintable files
    /// </summary>
    private void PrintPrintableFiles(IList<string> printFiles, AppStartParameters appStartParameters)
    {
        if (!string.IsNullOrEmpty(appStartParameters.Printer))
            _printFileHelper.PdfCreatorPrinter = appStartParameters.Printer;

        if (!_printFileHelper.AddFiles(printFiles, appStartParameters.Silent))
            return;

        var profileName = appStartParameters.Profile;

        _storedParametersManager.SaveParameterSettings(appStartParameters.OutputFile, profileName, printFiles.FirstOrDefault());
        _printFileHelper.PrintAll(appStartParameters.Silent);
    }

    private void HandleFiles(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters)
    {
        var directConversionFiles = new List<string>();
        var printFiles = new List<string>();
        var directImageConversionFiles = new List<string>();

        foreach (var file in droppedFiles)
        {
            if (_directConversion.IsDirectConversion(file))
                directConversionFiles.Add(file);
            else if (_directConversion.IsImageConversion(file))
                directImageConversionFiles.Add(file);
            else
                printFiles.Add(file);
        }

        var directConversionFilesList = new List<string>();
        foreach (var directConversionFile in directConversionFiles)
        {
            if (appStartParameters != null && appStartParameters.Merge)
                directConversionFilesList.Add(directConversionFile);
            else
                _directConversion.ConvertDirectly(new List<string>() { directConversionFile }, appStartParameters);
        }

        if (directConversionFilesList.Count > 0)
            _directConversion.ConvertDirectly(directConversionFilesList, appStartParameters);

        if (directImageConversionFiles.Count > 0)
            _directConversion.ConvertImagesDirectly(directImageConversionFiles, appStartParameters);

        if (printFiles.Any())
            PrintPrintableFiles(printFiles, appStartParameters);
    }
}
