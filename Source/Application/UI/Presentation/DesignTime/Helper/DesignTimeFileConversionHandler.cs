using System.Collections.Generic;
using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.Controller;
using pdfforge.PDFCreator.Core.DirectConversion;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

public class DesignTimeFileConversionHelper : IFileConversionHelper
{
    public void HandleFileList(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters)
    {
    }

    public void HandleFileListWithoutTooManyFilesWarning(IEnumerable<string> droppedFiles, AppStartParameters appStartParameters)
    {
    }

    public Task<JobInfo> GetJobInfoForPreviewMerge(IEnumerable<string> droppedFiles)
    {
        return null;
    }
}
