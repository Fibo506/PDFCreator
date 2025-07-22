using System.Windows.Controls;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.UI.Interactions;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Dialogs;

public class InputBoxWindowViewModel : OverlayViewModelBase<InputInteraction, InputBoxWindowTranslation>
{
    private string _validationMessage;

    public InputBoxWindowViewModel(ITranslationUpdater translationUpdater)
        : base(translationUpdater)
    {
        TextChangedCommand = new DelegateCommand<TextChangedEventArgs>(OnInputTextChanged);
        ConfirmDialogCommand = new DelegateCommand(ConfirmTextInput, CanConfirmTextInput);
        CancelDialogCommand = new DelegateCommand(CancelInputDialog);
    }

    public DelegateCommand<TextChangedEventArgs> TextChangedCommand { get; }
    public DelegateCommand ConfirmDialogCommand { get; }
    public DelegateCommand CancelDialogCommand { get; }

    public string ValidationMessage
    {
        get { return _validationMessage; }
        set
        {
            _validationMessage = value;
            RaisePropertyChanged(nameof(ValidationMessage));
        }
    }

    private void OnInputTextChanged(TextChangedEventArgs textChangedEventArgs)
    {
        if (Interaction.IsValidInput == null)
            return;

        ConfirmDialogCommand.RaiseCanExecuteChanged();
    }

    private void CancelInputDialog(object o)
    {
        Interaction.Success = false;
        FinishInteraction();
    }

    private bool CanConfirmTextInput(object o)
    {
        if (Interaction?.IsValidInput == null)
            return true;

        var validation = Interaction.IsValidInput(Interaction.InputText);
        ValidationMessage = validation.Message;
        return validation.IsValid;
    }

    private void ConfirmTextInput(object o)
    {
        Interaction.Success = true;
        FinishInteraction();
    }

    public override string Title => Interaction.Title;
}
