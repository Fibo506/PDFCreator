using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.DebugSettings;
using pdfforge.PDFCreator.Utilities.Threading;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

internal class DesignTimeDebugSettingsViewModel : DebugSettingsViewModel
{
    public DesignTimeDebugSettingsViewModel() : base(new TranslationUpdater(new TranslationFactory(null), new ThreadManager()), null, null)
    {
    }
}
