using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimeFreeEditionHotFolderViewModel : FreeEditionHotFolderViewModel
{
    public DesignTimeFreeEditionHotFolderViewModel() : base(
        new DesignTimeTranslationUpdater(),
        null)
    {
    }
}
