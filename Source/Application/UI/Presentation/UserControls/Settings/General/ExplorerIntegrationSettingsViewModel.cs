using System.Windows;
using System.Windows.Input;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.UI.Presentation.Assistants;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.General;

public class ExplorerIntegrationSettingsViewModel : AGeneralSettingsItemControlModel
{
    private readonly IOsHelper _osHelper;
    private readonly IUacAssistant _uacAssistant;

    public ExplorerIntegrationSettingsViewModel(IUacAssistant uacAssistant, IOsHelper osHelper, ITranslationUpdater translationUpdater, ICurrentSettingsProvider settingsProvider, IGpoSettings gpoSettings) :
        base(translationUpdater, settingsProvider, gpoSettings)
    {
        _uacAssistant = uacAssistant;
        _osHelper = osHelper;
        AddExplorerIntegrationCommand = new DelegateCommand(ExecuteAddToExplorerContextMenu);
        RemoveExplorerIntegrationCommand = new DelegateCommand(ExecuteRemoveFromExplorerContextMenu);
    }

    public Visibility RequiresUacVisibility => _osHelper.UserIsAdministrator() ? Visibility.Collapsed : Visibility.Visible;

    public bool IsAddedToExplorer { get; private set; }
    public bool IsRemovedFromExplorer { get; private set; }

    public ICommand AddExplorerIntegrationCommand { get; set; }
    public ICommand RemoveExplorerIntegrationCommand { get; set; }

    private void ExecuteAddToExplorerContextMenu(object obj)
    {
        IsAddedToExplorer = false;
        RaisePropertyChanged(nameof(IsAddedToExplorer));

        IsAddedToExplorer = _uacAssistant.AddExplorerIntegration();
        RaisePropertyChanged(nameof(IsAddedToExplorer));
    }

    private void ExecuteRemoveFromExplorerContextMenu(object obj)
    {
        IsRemovedFromExplorer = false;
        RaisePropertyChanged(nameof(IsRemovedFromExplorer));

        IsRemovedFromExplorer = _uacAssistant.RemoveExplorerIntegration();
        RaisePropertyChanged(nameof(IsRemovedFromExplorer));
    }
}
