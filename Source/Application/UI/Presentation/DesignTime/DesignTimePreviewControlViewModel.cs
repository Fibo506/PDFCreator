using pdfforge.PDFCreator.UI.Presentation.Controls;
using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimePreviewControlViewModel : PreviewControlViewModel
{
    public DesignTimePreviewControlViewModel() : base(null, new DesignTimeEditionHelper(), new DesignTimePreviewManager())

    {
    }
}
