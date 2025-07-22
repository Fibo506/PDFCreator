using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.General;
using pdfforge.PDFCreator.Utilities.Threading;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeGeneralSettingsViewModel : GeneralSettingsViewModel
{
    public DesignTimeGeneralSettingsViewModel() : base(new TranslationUpdater(new TranslationFactory(), new ThreadManager()))
    {
    }
}
