using System.Windows.Input;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.Conversion.Actions.Actions;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.SettingsManagement.DefaultSettings;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Web;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.SendActions.OpenFile;

public class OpenViewerActionViewModel : ActionViewModelBase<OpenFileAction, OpenViewerActionTranslation>
{
    private readonly IWebLinkLauncher _webLinkLauncher;
    private readonly IPdfEditorHelper _pdfEditorHelper;

    public bool UseDefaultViewer
    {
        get
        {
            if (CurrentProfile == null)
                return false;

            return !CurrentProfile.OpenViewer.OpenWithPdfArchitect &&
                    !CurrentProfile.OpenViewer.OpenFolder;
        }
        set
        {
            if (value)
            {
                CurrentProfile.OpenViewer.OpenWithPdfArchitect = false;
                CurrentProfile.OpenViewer.OpenFolder = false;
            }
            RaisePropertyChanged(nameof(UseDefaultViewer));
            RaisePropertyChanged(nameof(UsePdfArchitect));
            RaisePropertyChanged(nameof(OpenFolder));
        }
    }

    public bool UsePdfArchitect
    {
        get
        {
            if (CurrentProfile == null)
                return false;

            return CurrentProfile.OpenViewer.OpenWithPdfArchitect;
        }
        set
        {
            CurrentProfile.OpenViewer.OpenFolder = !value;
            CurrentProfile.OpenViewer.OpenWithPdfArchitect = value;
            RaisePropertyChanged(nameof(UseDefaultViewer));
            RaisePropertyChanged(nameof(UsePdfArchitect));
            RaisePropertyChanged(nameof(OpenFolder));
        }
    }

    public bool OpenFolder
    {
        get
        {
            if (CurrentProfile == null)
                return false;

            return CurrentProfile.OpenViewer.OpenFolder;
        }
        set
        {
            CurrentProfile.OpenViewer.OpenWithPdfArchitect = !value;
            CurrentProfile.OpenViewer.OpenFolder = value;
            RaisePropertyChanged(nameof(UseDefaultViewer));
            RaisePropertyChanged(nameof(UsePdfArchitect));
            RaisePropertyChanged(nameof(OpenFolder));
        }
    }

    public string OpenWithViewerTranslation => _pdfEditorHelper.UseSodaPdf ? Translation.FormatOpenWithCustomViewer("Soda PDF") : Translation.OpenWithPdfArchitect;
    public string MoreInfoOnEditorTranslation => _pdfEditorHelper.UseSodaPdf ? Translation.FormatEditorMoreInfo("Soda PDF") : Translation.FormatEditorMoreInfo("PDF Architect");

    public OpenViewerActionViewModel(ITranslationUpdater translationUpdater,
        IActionLocator actionLocator,
        ErrorCodeInterpreter errorCodeInterpreter,
        ICurrentSettingsProvider currentSettingsProvider,
        IDispatcher dispatcher,
        IDefaultSettingsBuilder defaultSettingsBuilder,
        IActionOrderHelper actionOrderHelper,
        IWebLinkLauncher webLinkLauncher,
        IPdfEditorHelper pdfEditorHelper)
        : base(actionLocator, errorCodeInterpreter, translationUpdater, currentSettingsProvider, dispatcher, defaultSettingsBuilder, actionOrderHelper)
    {
        _webLinkLauncher = webLinkLauncher;
        PdfArchitectInfoCommand = new DelegateCommand(ExecutePdfArchitectInfoCommand);
        _pdfEditorHelper = pdfEditorHelper;
    }

    private void ExecutePdfArchitectInfoCommand(object obj)
    {
        var url = _pdfEditorHelper.UseSodaPdf
            ? Urls.SodaPdfWebsiteUrl
            : Urls.ArchitectWebsiteUrl;

        _webLinkLauncher.Launch(url);
    }

    public ICommand PdfArchitectInfoCommand { get; }

    protected override string SettingsPreviewString
    {
        get
        {
            return CurrentProfile.OpenViewer switch
            {
                { OpenWithPdfArchitect: true } => OpenWithViewerTranslation,
                { OpenFolder: true } => Translation.OpenFolder,
                _ => Translation.OpenWithDefault
            };
        }
    }
}
