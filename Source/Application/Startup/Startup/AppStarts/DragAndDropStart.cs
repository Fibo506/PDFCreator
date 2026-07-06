using System.Collections.Generic;
using Newtonsoft.Json;
using pdfforge.PDFCreator.Core.Controller;

namespace pdfforge.PDFCreator.Core.Startup.AppStarts;

public class DragAndDropStart : MaybePipedStart
{
    private readonly IFileConversionHelper _fileConversionHelper;

    public DragAndDropStart(IFileConversionHelper fileConversionHelper, IMaybePipedApplicationStarter maybePipedApplicationStarter)
        : base(maybePipedApplicationStarter)
    {
        _fileConversionHelper = fileConversionHelper;
    }

    public IList<string> DroppedFiles { get; set; } = new List<string>();

    protected override string ComposePipeMessage()
    {
        var parameterJson = JsonConvert.SerializeObject(AppStartParameters);

        return "DragAndDrop|" + parameterJson + "|" + string.Join("|", DroppedFiles);
    }

    protected override bool StartApplication()
    {
        _fileConversionHelper.HandleFileList(DroppedFiles, AppStartParameters);
        return true;
    }
}
