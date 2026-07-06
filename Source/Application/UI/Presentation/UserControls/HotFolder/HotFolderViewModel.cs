using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using NaturalSort.Extension;
using pdfforge.Obsidian;
using pdfforge.Obsidian.Trigger;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.Startup.HotFolder;
using pdfforge.PDFCreator.UI.Presentation.Assistants;
using pdfforge.PDFCreator.UI.Presentation.Commands;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Services;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;
using pdfforge.PDFCreator.Utilities;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class HotFolderViewModel : TranslatableViewModelBase<HotFolderViewTranslation>, IMountable
{
    private readonly ICurrentSettings<HotFolderSettings> _hotFolderSettings;
    private readonly ICurrentSettings<ObservableCollection<ConversionProfile>> _profilesProvider;
    private readonly IHotFolderManager _hotFolderManager;
    private readonly IInteractionRequest _interactionRequest;
    private readonly IEventAggregator _eventAggregator;
    private readonly IGuid _guid;
    private readonly NaturalSortComparer _naturalSortComparer = new(StringComparison.CurrentCulture);
    private readonly IGpoSettings _gpoSettings;
    private readonly IPrinterAssistant _printerAssistant;
    private readonly IPrinterMappingService _printerMappingService;
    private readonly ErrorCodeInterpreter _errorCodeInterpreter;
    private readonly IAutoStartHelper _autoStartHelper;
    private readonly IHotFolderConfigChecker _hotFolderConfigChecker;


    public ObservableCollection<ConversionProfileWrapper> ConversionProfiles { get; private set; }
    public ObservableCollection<PrinterMappingWrapper> PrinterMappings { get; set; }
    public ObservableCollection<HotFolderDisplayItem> HotFolderDisplayItems { get; set; }

    public HotFolderSettings HotFolderSettings => _hotFolderSettings.Settings;
    private ListCollectionView _hotFolderCollectionView;

    public ICommand AddHotFolderCommand { get; private set; }
    public DelegateCommand EnableHotFolderCommand { get; }
    public DelegateCommand DeleteHotFolderCommand { get; }
    public DelegateCommand EditHotFolderConfigCommand { get; }
    public ICommand SaveSettingsCommand { get; }
    
    public HotFolderViewModel(
        ICurrentSettings<HotFolderSettings> hotFolderSettings,
        ICurrentSettings<ObservableCollection<ConversionProfile>> profilesProvider,
        IHotFolderManager hotFolderManager,
        IInteractionRequest interactionRequest,
        ITranslationUpdater translationUpdater,
        IGpoSettings gpoSettings,
        IPrinterAssistant printerAssistant,
        IPrinterMappingService printerMappingService,
        IHotFolderConfigChecker hotFolderConfigChecker,
        ErrorCodeInterpreter errorCodeInterpreter,
        ICommandLocator commandLocator,
        IEventAggregator eventAggregator,
        IGuid guid,
        IAutoStartHelper autoStartHelper)
        : base(translationUpdater)
    {
        _hotFolderSettings = hotFolderSettings;
        _profilesProvider = profilesProvider;
        _hotFolderManager = hotFolderManager;
        _interactionRequest = interactionRequest;
        _gpoSettings = gpoSettings;
        _printerAssistant = printerAssistant;
        _printerMappingService = printerMappingService;
        _hotFolderConfigChecker = hotFolderConfigChecker;
        _errorCodeInterpreter = errorCodeInterpreter;
        _eventAggregator = eventAggregator;
        _guid = guid;
        _autoStartHelper = autoStartHelper;

        SaveSettingsCommand = commandLocator.CreateMacroCommand()
            .AddCommand<ISaveChangedSettingsCommand>()
            .AddCommand<RaiseEditSettingsFinishedEventCommand>()
            .Build();

        AddHotFolderCommand = new DelegateCommand(AddHotFolderExecute);
        DeleteHotFolderCommand = new DelegateCommand(DeleteHotFolderExecute);
        EditHotFolderConfigCommand = new DelegateCommand(EditHotFolderConfigExecute);
        EnableHotFolderCommand = new DelegateCommand(EnableHotFolderExecute);
    }

    public void MountView()
    {
        SetupConversionProfiles();

        PrinterMappings = _printerMappingService.GetPrinterMappings();

        var hotFolderConfigs = _hotFolderSettings.Settings.HotFolderConfigs;

        HotFolderDisplayItems = hotFolderConfigs
            .OrderBy(x => x.Printer, StringComparison.OrdinalIgnoreCase.WithNaturalSort())
            .Select(hf => new HotFolderDisplayItem(_hotFolderConfigChecker, _errorCodeInterpreter)
            {
                HotFolderConfig = hf,
                PrinterMapping = PrinterMappings.FirstOrDefault(pm => pm.PrinterName == hf.Printer)
            })
            .ToObservableCollection();

        RaisePropertyChanged(nameof(HotFolderDisplayItems));
        _hotFolderCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(HotFolderDisplayItems);

        Comparison<HotFolderDisplayItem> displayItemComparison = (hfX, hfY)
            => _naturalSortComparer.Compare(hfX.Printer, hfY.Printer);
        var displayItemComparer = Comparer<HotFolderDisplayItem>.Create(displayItemComparison);
        _hotFolderCollectionView.CustomSort = displayItemComparer;

        RaisePropertyChanged(nameof(ShouldEnableOnOffSwitch));

        if (HotFolderDisplayItems.Count == 0 && IsHotFolderEnabled)
        {
            IsHotFolderEnabled = false;
        }
    }

    public bool IsHotFolderEnabled
    {
        get => HotFolderSettings.IsEnabled;
        set
        {
            if (HotFolderSettings.IsEnabled != value)
            {
                HotFolderSettings.IsEnabled = value;
                if (value)
                {
                    _hotFolderManager.StartAll();
                }
                else
                {
                    _hotFolderManager.StopAll();
                }

                SaveSettings();
                _eventAggregator.GetEvent<HotFolderStatusChangedEvent>().Publish(value);
            }
            RaisePropertyChanged();
        }
    }

    public void UnmountView()
    {
    }

    private async void AddHotFolderExecute(object o)
    {
        var hotFolderPrinterName = await _printerAssistant.AddPrinter("HotFolder");
        if (hotFolderPrinterName == null)
            return;

        var guid = _guid.NewGuidString();
        var newHotFolder = new HotFolderConfig { Guid = guid, Printer = hotFolderPrinterName };
        _hotFolderSettings.Settings.HotFolderConfigs.Add(newHotFolder);

        var printerMappingWrapper = new PrinterMappingWrapper(
            new PrinterMapping(hotFolderPrinterName, ProfileGuids.DEFAULT_PROFILE_GUID, true),
            ConversionProfiles);

        PrinterMappings.Add(printerMappingWrapper);

        var displayItem = new HotFolderDisplayItem(_hotFolderConfigChecker, _errorCodeInterpreter)
        {
            HotFolderConfig = newHotFolder,
            PrinterMapping = printerMappingWrapper
        };

        HotFolderDisplayItems.Add(displayItem);
        RaisePropertyChanged(nameof(ShouldEnableOnOffSwitch));
        RaisePropertyChanged(nameof(HotFolderDisplayItems));
        SaveSettings();

        var interaction = new EditHotFolderUserInteraction(newHotFolder, printerMappingWrapper, ConversionProfiles);
        await _interactionRequest.RaiseAsync(interaction);

        if (interaction.Success)
        {
            printerMappingWrapper.Profile = interaction.ResultProfile;
            RaisePropertyChanged(nameof(HotFolderDisplayItems));
            _hotFolderCollectionView.Refresh();
            SaveSettings();
        }
    }

    private async void DeleteHotFolderExecute(object obj)
    {
        HotFolderDisplayItem displayItem = null;

        if (obj is HotFolderDisplayItem item)
            displayItem = item;
        else if (_hotFolderCollectionView.CurrentItem is HotFolderDisplayItem currentItem)
            displayItem = currentItem;

        if (displayItem?.HotFolderConfig == null)
            return;

        var hotFolderConfig = displayItem.HotFolderConfig;

        if (!await _printerAssistant.DeletePrinter(hotFolderConfig.Printer, 3))
            return;

        HotFolderDisplayItems.Remove(displayItem);
        _hotFolderSettings.Settings.HotFolderConfigs.Remove(hotFolderConfig);
        RaisePropertyChanged(nameof(ShouldEnableOnOffSwitch));

        var mappingToRemove = PrinterMappings.FirstOrDefault(pm => pm.PrinterName == hotFolderConfig.Printer);
        if (mappingToRemove != null)
        {
            PrinterMappings.Remove(mappingToRemove);
        }

        if (HotFolderDisplayItems.Count == 0)
        {
            IsHotFolderEnabled = false;
        }

        SaveSettings();
        _hotFolderManager.Stop(hotFolderConfig);
    }

    private async void EditHotFolderConfigExecute(object obj)
    {
        if (obj is not HotFolderConfig hotFolderConfig)
            return;

        var printerMappingWrapper = PrinterMappings.FirstOrDefault(pr => pr.PrinterName == hotFolderConfig.Printer);
        var interaction = new EditHotFolderUserInteraction(hotFolderConfig, printerMappingWrapper, ConversionProfiles);
        await _interactionRequest.RaiseAsync(interaction);

        if (interaction.Success)
        {
            printerMappingWrapper!.PrinterName = interaction.HotFolderConfig.Printer;
            printerMappingWrapper.Profile = interaction.ResultProfile;
            printerMappingWrapper.IsHotFolder = true; // Ensure the printer's marked as HotFolder (in case it was changed, for example, during migration)
            RaisePropertyChanged(nameof(HotFolderDisplayItems));
            _hotFolderCollectionView.Refresh();
            SaveSettings();

            _hotFolderManager.UpdateSetting(hotFolderConfig);
        }
    }

    private void EnableHotFolderExecute(object obj)
    {
        HotFolderDisplayItem displayItem = null;

        if (obj is HotFolderDisplayItem item)
            displayItem = item;
        else if (_hotFolderCollectionView.CurrentItem is HotFolderDisplayItem currentItem)
            displayItem = currentItem;

        var wasActive = displayItem!.HotFolderConfig.IsActive;
        displayItem!.HotFolderConfig.IsActive = !displayItem.HotFolderConfig.IsActive;

        RaisePropertyChanged(nameof(HotFolderDisplayItems));
        RaisePropertyChanged(nameof(ShouldEnableOnOffSwitch));

        if (wasActive)
        {
            _hotFolderManager.Stop(displayItem!.HotFolderConfig);
        }
        else
        {
            if(_hotFolderSettings.Settings.IsEnabled)
                _hotFolderManager.Start(displayItem!.HotFolderConfig);
        }

        SaveSettings();
    }

    private void SetupConversionProfiles()
    {
        var conversionProfiles =
            _profilesProvider.Settings.Select(x => new ConversionProfileWrapper(x))
                .OrderBy(pm => pm.Name, StringComparison.OrdinalIgnoreCase.WithNaturalSort())
                .ToObservableCollection();

        ConversionProfiles = conversionProfiles;
    }

    private void SaveSettings()
    {
        SaveSettingsCommand.Execute(null);
    }

    public bool HotFolderIsDisabledByGpo
    {
        get
        {
            if (_profilesProvider.Settings == null)
                return false;

            return _gpoSettings is { LoadSharedHotFolders: true };
        }
    }

    public bool StartWithWindows
    {
        get => _autoStartHelper.IsActive();
        set
        {
            if (StartWithWindows == value)
                return;

            if (value)
            {
                _autoStartHelper.Register();
            }
            else
            {
                _autoStartHelper.UnRegister();
            }
        }
    }

    public bool StartWithAppStart
    {
        get => HotFolderSettings.StartWithAppStart;
        set
        {
            if (HotFolderSettings.StartWithAppStart != value)
            {
                HotFolderSettings.StartWithAppStart = value;
                RaisePropertyChanged();
                SaveSettings();
            }
        }
    }

    public bool ShouldEnableOnOffSwitch => HotFolderDisplayItems != null && HotFolderDisplayItems.Count != 0;
}
