using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.ModifyActions.Signature;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeSignaturePasswordOverlayViewModel : SignaturePasswordOverlayViewModel
{
    public DesignTimeSignaturePasswordOverlayViewModel() : base(new DesignTimeTranslationUpdater(), new DesignTimeSignaturePasswordCheck())
    {
    }
}
