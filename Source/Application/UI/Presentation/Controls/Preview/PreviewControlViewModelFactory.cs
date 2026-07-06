using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;


public interface IPreviewControlViewModelFactory
{
    PreviewControlViewModel Create(JobInfo jobInfo);
}

public class PreviewControlViewModelFactory : IPreviewControlViewModelFactory
{
    private readonly IPreviewManager _previewManager;
    private readonly IPreviewPageControlViewModelFactory _previewPageFactory;
    private readonly EditionHelper _editionHelper;

    public PreviewControlViewModelFactory(IPreviewManager previewManager, IPreviewPageControlViewModelFactory previewPageFactory, EditionHelper editionHelper)
    {
        _previewManager = previewManager;
        _previewPageFactory = previewPageFactory;
        _editionHelper = editionHelper;
    }
    public PreviewControlViewModel Create(JobInfo jobInfo)
    {
        return new PreviewControlViewModel(_previewPageFactory, _editionHelper, _previewManager)
        {
            JobInfo = jobInfo
        };
    }
}
