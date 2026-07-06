using System;
using System.Collections.Generic;
using System.Threading;
using NLog;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;
using pdfforge.PDFCreator.Core.Startup.HotFolder;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.UI.Presentation.Events.HotFolder;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.Utilities.Threading;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

public class HotFolderManager : IHotFolderManager
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IHotFolderObserverManager _hotFolderObserverManager;
    private readonly ISettingsProvider _settingsProvider;
    private ISynchronizedThread _hotFolderThread;
    private readonly IThreadManager _threadManager;
    private readonly IEventAggregator _eventAggregator;
    private bool _isRunning;

    public HotFolderManager(
        IHotFolderObserverManager folderObserverManagerManager,
        ISettingsProvider settingsProvider,
        IThreadManager threadManager,
        IEventAggregator eventAggregator)
    {
        _hotFolderObserverManager = folderObserverManagerManager;
        _settingsProvider = settingsProvider;
        _threadManager = threadManager;
        _eventAggregator = eventAggregator;
        _settingsProvider.SettingsChanged += SettingsProviderOnSettingsChanged;
    }

    private void SettingsProviderOnSettingsChanged(object sender, EventArgs e)
    {
        // check if settings are already loaded
        if (_settingsProvider.Settings.ConversionProfiles.Count > 0)
            StartAll();
    }

    public void StartAll()
    {
        _logger.Info("Try start logging");
        // when settings are not loaded yet the number of ConversionProfiles will be 0,
        // to make sure settings are fully loaded when StartAll is called we check for the numbers of ConversionProfiles
        if (_isRunning || !_settingsProvider.Settings.HotFolderSettings.IsEnabled || _settingsProvider.Settings.ConversionProfiles.Count == 0)
        {
            var reasonForNotStarting = _isRunning ? "HotFolder are already running." : "HotFolder feature is not activated."; 
            _logger.Warn($"Failed to start all HotFolder.{reasonForNotStarting}");
            return;
        }

        _isRunning = true;
        _hotFolderThread = new SynchronizedThread(StartAllObserver) { Name = "HotFolderThread" };
        _hotFolderThread.SetApartmentState(ApartmentState.Unknown);
        _threadManager.StartSynchronizedThread(_hotFolderThread);
    }

    private void StartAllObserver()
    {
        _hotFolderObserverManager.StartWatchingAll();
        _logger.Info("HotFolders started");
    }

    public void Stop(HotFolderConfig config)
    {
        _logger.Info($"Stop HotFolder {config.Guid} with printer name {config.Printer} and target path {config.HotFolderPath}");
        _hotFolderObserverManager.StopWatching(config);
    }

    public void Start(HotFolderConfig config)
    {
        _logger.Info($"Start HotFolder {config.Guid} with printer name {config.Printer} and target path {config.HotFolderPath}");
        _hotFolderObserverManager.StartWatching(config);
    }

    public void UpdateSetting(HotFolderConfig config)
    {
        if (_hotFolderObserverManager.IsRunning(config))
            _hotFolderObserverManager.StopWatching(config);
        _hotFolderObserverManager.StartWatching(config);
        _logger.Info($"Update HotFolder {config.Guid} with printer name {config.Printer} and target path {config.HotFolderPath}");
    }

    public void StopAll()
    {
        _hotFolderObserverManager.StopWatchingAll();

        _isRunning = false;
        _eventAggregator.GetEvent<StartHotFolderEvent>().Unsubscribe(Start);
        _eventAggregator.GetEvent<StartHotFolderEvent>().Unsubscribe(Stop);

        _logger.Info("Stopped All HotFolders");
    }
}
