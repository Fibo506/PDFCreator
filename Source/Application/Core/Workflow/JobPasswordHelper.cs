using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Core.Workflow;

public class JobPasswordHelper
{
    public static JobPasswords GetJobPasswords(ConversionProfile profile, Accounts accounts)
    {
        var passwords = new JobPasswords();

        passwords.PdfOwnerPassword = profile.PdfSettings.Security.OwnerPassword;
        passwords.PdfUserPassword = profile.PdfSettings.Security.UserPassword;
        passwords.PdfSignaturePassword = profile.PdfSettings.Signature.SignaturePassword;

        passwords.FtpPassword = accounts.GetFtpAccount(profile)?.Password ?? "";
        passwords.SmtpPassword = accounts.GetSmtpAccount(profile)?.Password ?? "";
        passwords.HttpPassword = accounts.GetHttpAccount(profile)?.Password ?? "";

        return passwords;
    }
}
