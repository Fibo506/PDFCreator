using System.Collections.Generic;
using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.DirectConversion;

namespace pdfforge.PDFCreator.Core.Controller;

public interface IFileConversionHelper
{
    /// <summary>
    ///     Removes invalid files and launches print jobs for the files that needs to be printed.
    ///     If successful, the direct convertible files are added to the current JobInfoQueue.
    /// </summary>
    void HandleFileList(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters);

    void HandleFileListWithoutTooManyFilesWarning(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters);

    Task<JobInfo> GetJobInfoForPreviewMerge(IEnumerable<string> droppedFiles);
}
