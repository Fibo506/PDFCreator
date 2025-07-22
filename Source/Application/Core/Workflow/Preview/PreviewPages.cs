using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pdfforge.PDFCreator.Core.Workflow;

public class PreviewPage
{
    public int PageNumber { get; set; }
    public Task<string> PreviewImagePathTask { get; set; }
    public int SourcePageNumber { get; set; }
    public int RotationAngle { get; set; }
    public bool IsExcluded { get; set; }

    public PreviewPage(int pageNumber, Task<string> previewImagePathTask)
    {
        PreviewImagePathTask = previewImagePathTask;
        PageNumber = pageNumber;
        RotationAngle = 0;
        IsExcluded = false;
    }
}

public class PreviewPages
{
    public string Directory { get; }
    public IList<PreviewPage> PreviewPageList { get; set; } = new List<PreviewPage>();
    public Action DisposeDocument { get; set; }

    public PreviewPages(string directory)
    {
        Directory = directory;
    }
}
