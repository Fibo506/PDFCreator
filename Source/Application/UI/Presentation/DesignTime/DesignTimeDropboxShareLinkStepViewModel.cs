using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeDropboxShareLinkStepViewModel : DropboxShareLinkStepViewModel
{
    public DesignTimeDropboxShareLinkStepViewModel() : base(new DesignTimeTranslationUpdater(), new DesignTimeCommandLocator())
    {
        ShareUrl = "https://www.dropbox.com/test/SharedFile.pdf";
    }
}
