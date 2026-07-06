using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Commands;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class FreeEditionHotFolderViewModel : TranslatableViewModelBase<HotFolderViewTranslation>
{
    public ICommand UrlOpenCommand { get; }

    public FreeEditionHotFolderViewModel(ITranslationUpdater translationUpdater,
        UrlOpenCommand urlOpenCommand) : base(translationUpdater)
    {
        UrlOpenCommand = urlOpenCommand;
    }

    public string ShopUrl => Urls.UpgradeForHotFolder;
}

