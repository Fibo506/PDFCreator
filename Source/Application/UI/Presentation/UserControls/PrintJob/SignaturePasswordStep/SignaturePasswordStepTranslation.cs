namespace pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;

public class SignaturePasswordStepTranslation : PasswordButtonControlTranslation
{
    public string SignaturePasswordTitle { get; private set; } = "Set Certificate password";
    public string CertificatePassword { get; private set; } = "Certificate password:";
    public string CertificateFile { get; private set; } = "Certificate file:";
    public string IncorrectPassword { get; private set; } = "The entered password is not valid for this certificate";
}
