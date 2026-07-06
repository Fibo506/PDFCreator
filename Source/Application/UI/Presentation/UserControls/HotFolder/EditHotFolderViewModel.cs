using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using pdfforge.Obsidian;
using pdfforge.Obsidian.Trigger;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.UI.Presentation.Assistants;
using pdfforge.PDFCreator.UI.Presentation.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper.Tokens;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class EditHotFolderViewModel : OverlayViewModelBase<EditHotFolderUserInteraction, EditHotFolderTranslatable>, IMountable
{
    private readonly IInteractionRequest _interactionRequest;
    private readonly IPrinterAssistant _printerAssistant;
    private readonly ErrorCodeInterpreter _errorCodeInterpreter;
    private readonly IHotFolderConfigChecker _hotFolderConfigChecker;
    private readonly ITokenViewModelFactory _tokenViewModelFactory;
    private readonly ITokenHelper _tokenHelper;
    private readonly ITokenButtonFunctionProvider _tokenButtonFunctionProvider;

    private HotFolderConfig _originalHotFolderConfig;
    private HotFolderConfig _workingCopy;
    private ConversionProfileWrapper _selectedProfile;
    private ObservableCollection<string> _filters;
    private bool _isInitialized = false;
    public override string Title { get; }
    public ObservableCollection<ConversionProfileWrapper> Profiles { get; set; }
    public AsyncCommand SaveCommand { get; }
    public DelegateCommand AddFilterCommand { get; }
    public DelegateCommand<string> RemoveFilterCommand { get; }
    public TokenViewModel<HotFolderConfig> HotFolderPathTokenViewModel { get; private set; }
    public TokenViewModel<HotFolderConfig> SourceFilesPathTokenViewModel { get; private set; }
    public TokenViewModel<HotFolderConfig> UnprintableFilesPathTokenViewModel { get; private set; }

    public EditHotFolderViewModel(ITranslationUpdater translationUpdater,
        IInteractionRequest interactionRequest,
        IPrinterAssistant printerAssistant,
        ErrorCodeInterpreter errorCodeInterpreter,
        IHotFolderConfigChecker hotFolderConfigChecker,
        ITokenViewModelFactory tokenViewModelFactory,
        ITokenHelper tokenHelper,
        ITokenButtonFunctionProvider tokenButtonFunctionProvider
        ) : base(translationUpdater)
    {
        _interactionRequest = interactionRequest;
        _printerAssistant = printerAssistant;
        _errorCodeInterpreter = errorCodeInterpreter;
        _hotFolderConfigChecker = hotFolderConfigChecker;
        _tokenViewModelFactory = tokenViewModelFactory;
        _tokenHelper = tokenHelper;
        _tokenButtonFunctionProvider = tokenButtonFunctionProvider;

        Title = Translation.EditHotFolderTitle;

        SaveCommand = new AsyncCommand(o => SaveExecute(), o => SaveCanExecute());

        AddFilterCommand = new DelegateCommand(o => AddFilter());
        RemoveFilterCommand = new DelegateCommand<string>(RemoveFilter);
    }

    private void SetTokenViewModels()
    {
        var builder = _tokenViewModelFactory
            .Builder<HotFolderConfig>();

        var tokenReplacer = _tokenHelper.TokenReplacerWithPlaceHolders;

        HotFolderPathTokenViewModel = builder
            .WithSelector(hfc => hfc.HotFolderPath)
            .WithInitialValue(_workingCopy)
            .WithTokenList(th => th.GetTokenListForHotFolderPaths())
            .WithTokenCustomPreview(s => ValidName.MakeValidFileName(tokenReplacer.ReplaceTokens(s)))
            .WithDefaultTokenReplacerPreview()
            .WithButtonCommand(_tokenButtonFunctionProvider.GetBrowseFolderFunction(Translation.SelectWatchFolder))
            .Build();
        RaisePropertyChanged(nameof(HotFolderPathTokenViewModel));

        SourceFilesPathTokenViewModel = builder
            .WithSelector(p => p.SourceFilesPath)
            .WithInitialValue(_workingCopy)
            .WithTokenList(th => th.GetTokenListForHotFolderPaths())
            .WithTokenCustomPreview(s => ValidName.MakeValidFolderName(tokenReplacer.ReplaceTokens(s)))
            .WithButtonCommand(_tokenButtonFunctionProvider.GetBrowseFolderFunction(Translation.SelectSourceFilesFolder))
            .Build();
        RaisePropertyChanged(nameof(SourceFilesPathTokenViewModel));

        UnprintableFilesPathTokenViewModel = builder
            .WithSelector(p => p.UnprintableFilesPath)
            .WithInitialValue(_workingCopy)
            .WithTokenList(th => th.GetTokenListForHotFolderPaths())
            .WithTokenCustomPreview(s => ValidName.MakeValidFolderName(tokenReplacer.ReplaceTokens(s)))
            .WithButtonCommand(_tokenButtonFunctionProvider.GetBrowseFolderFunction(Translation.SelectUnprintableFilesFolder))
            .Build();
        RaisePropertyChanged(nameof(UnprintableFilesPathTokenViewModel));
    }
    protected override void HandleInteractionObjectChanged()
    {
        _isInitialized = false;

        _originalHotFolderConfig = Interaction.HotFolderConfig;
        _workingCopy = CreateCopy(_originalHotFolderConfig);
        Profiles = Interaction.ProfileWrappers;

        _selectedProfile = Profiles?.FirstOrDefault(wrapper => wrapper.Name == Interaction.PrinterMappingWrapper.Profile.Name);
        _filters = new ObservableCollection<string>(_workingCopy.Filter);

        RaisePropertyChanged(nameof(PrinterName));
        RaisePropertyChanged(nameof(SourceFileMover));
        RaisePropertyChanged(nameof(UnprintableFileMover));
        RaisePropertyChanged(nameof(ShowSourceFilesPath));
        RaisePropertyChanged(nameof(ShowUnprintableFilesPath));
        RaisePropertyChanged(nameof(FilterOption));
        RaisePropertyChanged(nameof(ShowFilterOptions));
        RaisePropertyChanged(nameof(Profiles));
        RaisePropertyChanged(nameof(SelectedProfile));
        RaisePropertyChanged(nameof(Filters));

        SetTokenViewModels();

        _isInitialized = true;

        SaveCommand.RaiseCanExecuteChanged();
    }

    public bool ShowSourceFilesPath => SourceFileMover == Conversion.Settings.HotFolder.Enums.FileMover.MoveToLocation;
    public bool ShowUnprintableFilesPath => UnprintableFileMover == Conversion.Settings.HotFolder.Enums.FileMover.MoveToLocation;
    public bool ShowFilterOptions => FilterOption != Conversion.Settings.HotFolder.Enums.FilterOption.NoFilter;

    public bool PrinterNameChanged => _workingCopy?.Printer != _originalHotFolderConfig?.Printer;

    public string PrinterName
    {
        get { return _workingCopy?.Printer; }
        set
        {
            _workingCopy.Printer = value;
            SaveCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(PrinterNameChanged));
        }
    }

    public Conversion.Settings.HotFolder.Enums.FileMover SourceFileMover
    {
        get { return _workingCopy?.SourceFileMover ?? Conversion.Settings.HotFolder.Enums.FileMover.Subfolder; }
        set
        {
            if (_workingCopy != null)
            {
                _workingCopy.SourceFileMover = value;
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(SourceFileMover));
                RaisePropertyChanged(nameof(ShowSourceFilesPath));
            }
        }
    }

    public pdfforge.PDFCreator.Conversion.Settings.HotFolder.Enums.FileMover UnprintableFileMover
    {
        get { return _workingCopy?.UnprintableFileMover ?? Conversion.Settings.HotFolder.Enums.FileMover.Subfolder; }
        set
        {
            if (_workingCopy != null)
            {
                _workingCopy.UnprintableFileMover = value;
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(UnprintableFileMover));
                RaisePropertyChanged(nameof(ShowUnprintableFilesPath));
            }
        }
    }

    private string _statusText;

    public string StatusText
    {
        get => _statusText;
        set
        {
            if (value == _statusText) return;
            _statusText = value;
            RaisePropertyChanged(nameof(StatusText));
        }
    }

    public pdfforge.PDFCreator.Conversion.Settings.HotFolder.Enums.FilterOption FilterOption
    {
        get { return _workingCopy?.FilterOption ?? Conversion.Settings.HotFolder.Enums.FilterOption.NoFilter; }
        set
        {
            if (_workingCopy != null)
            {
                _workingCopy.FilterOption = value;
                SaveCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(FilterOption));
                RaisePropertyChanged(nameof(ShowFilterOptions));
            }
        }
    }

    public ObservableCollection<string> Filters
    {
        get => _filters;
        set
        {
            _filters = value;
            RaisePropertyChanged(nameof(Filters));
        }
    }


    public ConversionProfileWrapper SelectedProfile
    {
        get { return _selectedProfile; }
        set
        {
            if (value != null)
            {
                _selectedProfile = value;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private async void AddFilter()
    {
        var filterEditInteraction = new AddHotFolderFilterUserInteraction("");
        await _interactionRequest.RaiseAsync(filterEditInteraction);

        if (filterEditInteraction.Success && !string.IsNullOrWhiteSpace(filterEditInteraction.FileExtension))
        {
            Filters.Add(filterEditInteraction.FileExtension);
            SaveCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(Filters));
        }
    }

    private void RemoveFilter(string filter)
    {
        if (filter != null && Filters.Contains(filter))
        {
            Filters.Remove(filter);
            SaveCommand.RaiseCanExecuteChanged();
        }
    }

    private async Task<bool> RenamePrinter(HotFolderConfig workingCopy, HotFolderConfig originalHotFolderConfig)
    {
        var oldPrinterName = originalHotFolderConfig.Printer;
        var newPrinterName = workingCopy?.Printer;

        newPrinterName = await _printerAssistant.ApplyNewPrinterName(newPrinterName, oldPrinterName);

        if (string.IsNullOrEmpty(newPrinterName))
            return false;

        _workingCopy!.Printer = newPrinterName;
        return true;
    }


    public async Task<bool> SaveExecute()
    {
        if (PrinterNameChanged)
        {
            var result = await RenamePrinter(_workingCopy, _originalHotFolderConfig);
            if (!result)
                return false;
        }

        ApplyChangesToOriginal(_originalHotFolderConfig, _workingCopy);
        Interaction.HotFolderConfig = _originalHotFolderConfig;
        Interaction.ResultProfile = _selectedProfile;
        Interaction.Success = true;
        FinishInteraction();

        return true;
    }

    public bool SaveCanExecute()
    {
        if (!_isInitialized)
            return false;

        var checkResult = _hotFolderConfigChecker.CheckForEditingConfig(_workingCopy, SelectedProfile.ConversionProfile);

        if (!checkResult)
            StatusText = _errorCodeInterpreter.GetFirstErrorText(checkResult, false);
        else
            StatusText = string.Empty;

        return checkResult;
    }

    private HotFolderConfig CreateCopy(HotFolderConfig original)
    {
        return new HotFolderConfig
        {
            FilterOption = original.FilterOption,
            Filter = new List<string>(original.Filter ?? new List<string>()),
            HotFolderPath = original.HotFolderPath,
            IsActive = original.IsActive,
            Guid = original.Guid,
            Printer = original.Printer,
            SourceFileMover = original.SourceFileMover,
            SourceFilesPath = original.SourceFilesPath,
            UnprintableFileMover = original.UnprintableFileMover,
            UnprintableFilesPath = original.UnprintableFilesPath
        };
    }
    private void ApplyChangesToOriginal(HotFolderConfig original, HotFolderConfig workingCopy)
    {
        original.FilterOption = workingCopy.FilterOption;
        original.Filter = Filters?.ToList() ?? new List<string>();
        original.HotFolderPath = workingCopy.HotFolderPath;
        original.IsActive = workingCopy.IsActive;
        original.Guid = workingCopy.Guid;
        original.Printer = workingCopy.Printer;
        original.SourceFileMover = workingCopy.SourceFileMover;
        original.SourceFilesPath = workingCopy.SourceFilesPath;
        original.UnprintableFileMover = workingCopy.UnprintableFileMover;
        original.UnprintableFilesPath = workingCopy.UnprintableFilesPath;
    }

    private void HotFolderConfigChanged(object sender, PropertyChangedEventArgs e)
    {
        SaveCommand.RaiseCanExecuteChanged();
    }

    public void MountView()
    {
        _workingCopy.PropertyChanged -= HotFolderConfigChanged;
        _workingCopy.PropertyChanged += HotFolderConfigChanged;

        HotFolderPathTokenViewModel.MountView();
        SourceFilesPathTokenViewModel.MountView();
        UnprintableFilesPathTokenViewModel.MountView();
    }

    public void UnmountView()
    {
        _workingCopy.PropertyChanged -= HotFolderConfigChanged;

        HotFolderPathTokenViewModel.UnmountView();
        SourceFilesPathTokenViewModel.UnmountView();
        UnprintableFilesPathTokenViewModel.UnmountView();
    }
}
