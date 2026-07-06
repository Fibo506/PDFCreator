using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimeHotFolderViewModel : HotFolderViewModel
{
    public DesignTimeHotFolderViewModel() : base(
        null,
        null,
        null,
        null,
        new DesignTimeTranslationUpdater(),
        null,
        null,
        null,
        null,
        new DesignTimeErrorCodeInterpreter(),
        new DesignTimeCommandLocator(),
        new DesignTimeEventAggregator(),
        new GuidWrap(),
        null)
    { }
}
