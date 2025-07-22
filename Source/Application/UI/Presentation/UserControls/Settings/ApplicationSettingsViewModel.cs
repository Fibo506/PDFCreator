using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.UI.Presentation.Events;
using Prism.Events;
using Prism.Mvvm;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings;

public class ApplicationSettingsViewModel : BindableBase
{
    private readonly IGpoSettings _gpoSettings;
    private readonly IEventAggregator _eventAggregator;

    public ApplicationSettingsViewModel(IGpoSettings gpoSettings, IEventAggregator eventAggregator)
    {
        _gpoSettings = gpoSettings;
        _eventAggregator = eventAggregator;
        _eventAggregator.GetEvent<NavigateApplicationSettingsEvent>().Subscribe(targetView =>
        {
            _activePath = targetView;
            RaisePropertyChanged(nameof(ActivePathIsLockedByGpo));
        });
    }

    private string _activePath = "";

    public bool ApplicationSettingsIsDisabled => _gpoSettings is { DisableApplicationSettings: true };

    public bool ActivePathIsLockedByGpo
    {
        get
        {
            if (_gpoSettings == null)
                return false;

            if (_activePath.Equals(RegionViewName.TitleReplacementsRegionView))
                return _gpoSettings.DisableTitleTab;
            if (_activePath.Equals(RegionViewName.DebugSettingRegionView))
                return _gpoSettings.DisableDebugTab;

            return false;
        }
    }

}
