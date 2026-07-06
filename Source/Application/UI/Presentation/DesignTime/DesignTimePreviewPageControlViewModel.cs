using pdfforge.PDFCreator.UI.Presentation.Controls;
using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimePreviewPageControlViewModel : PreviewPageControlViewModel
{
    public DesignTimePreviewPageControlViewModel() : base(new DesignTimeTranslationUpdater(), new DesignTimeInteractionInvoker())
    {
    }
}
