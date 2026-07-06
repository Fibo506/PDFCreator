using System.Linq;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class AddHotFolderFilterViewModel : OverlayViewModelBase<AddHotFolderFilterUserInteraction, EditHotFolderTranslatable>
{
    private readonly IPathUtil _pathUtil;
    private string _filterText;
    private bool _isExtensionInvalid;
    private string _errorMessage;

    public AddHotFolderFilterViewModel(ITranslationUpdater translationUpdater, IPathUtil pathUtil) : base(translationUpdater)
    {
        _pathUtil = pathUtil;
        Title = Translation.AddFileExtensionFilterTitle;

        SaveCommand = new DelegateCommand(o => Save(), o => CanSave());
        CancelCommand = new DelegateCommand(o => Cancel());
    }

    public override string Title { get; }
    public DelegateCommand SaveCommand { get; }
    public DelegateCommand CancelCommand { get; }


    public string FilterText
    {
        get => _filterText;
        set
        {
            // normalize: if user enters "txt", turn it into ".txt"
            if (!string.IsNullOrWhiteSpace(value))
            {
                _filterText = value.StartsWith(".") ? value : "." + value;
            }
            else
            {
                _filterText = value;
            }

            ValidateExtension();
            RaisePropertyChanged(nameof(FilterText));
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
    public bool IsExtensionInvalid
    {
        get => _isExtensionInvalid;
        set
        {
            _isExtensionInvalid = value;
            RaisePropertyChanged(nameof(IsExtensionInvalid));
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            RaisePropertyChanged(nameof(ErrorMessage));
        }
    }

    protected override void HandleInteractionObjectChanged()
    {
        FilterText = Interaction.FileExtension;
    }

    private void Save()
    {
        var normalized = string.IsNullOrWhiteSpace(FilterText)
            ? string.Empty
            : (FilterText.StartsWith(".") ? FilterText : "." + FilterText);

        Interaction.FileExtension = normalized;
        Interaction.Success = true;
        FinishInteraction();
    }

    private void ValidateExtension()
    {
        if (string.IsNullOrWhiteSpace(FilterText))
        {
            IsExtensionInvalid = true;
            ErrorMessage = string.Empty;
            return;
        }

        if (FilterText.Any(char.IsWhiteSpace) || !_pathUtil.IsValidFilename(FilterText))
        {
            IsExtensionInvalid = true;
            ErrorMessage = Translation.FilterNotValid;
            return;
        }

        IsExtensionInvalid = false;
        ErrorMessage = string.Empty;
    }

    private bool CanSave()
    {
        return !IsExtensionInvalid && !string.IsNullOrWhiteSpace(FilterText);
    }

    private void Cancel()
    {
        Interaction.Success = false;
        FinishInteraction();
    }
}
