using pdfforge.PDFCreator.UI.Presentation.DesignTime;
using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Accounts.AccountViews.Microsoft;

public class DesignTimeMicrosoftAccountViewModel : MicrosoftAccountViewModel
{
    public DesignTimeMicrosoftAccountViewModel() : base(new DesignTimeTranslationUpdater(), new DesignTimeCommandLocator(), null, new DesignTimeEditionHelper())
    {
    }

}
