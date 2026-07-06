using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimeEditHotFolderViewModel : EditHotFolderViewModel
{
    public DesignTimeEditHotFolderViewModel() : base(new DesignTimeTranslationUpdater(),
        new DesignTimeInteractionRequest(),
        null,
        new DesignTimeErrorCodeInterpreter(),
        null,
        new DesignTimeTokenViewModelFactory(),
        new DesignTimeTokenHelper(),
        null)
    {
    }
}
