namespace pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;

public class SecurityPasswordsStepTranslation : PasswordButtonControlTranslation
{
    public string OwnerPasswordLabelContent { get; protected set; } = "Owner password (for editing):";
    public string SecurityTitle { get; protected set; } = "Security";
    public string UserPasswordLabelContent { get; protected set; } = "User password (for opening):";
}
