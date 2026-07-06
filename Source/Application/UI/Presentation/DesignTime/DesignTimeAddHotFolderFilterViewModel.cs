using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimeAddHotFolderFilterViewModel : AddHotFolderFilterViewModel
{
    public DesignTimeAddHotFolderFilterViewModel() : base(new DesignTimeTranslationUpdater(),
        new DesignTimePathUtil())
    {
    }
}
