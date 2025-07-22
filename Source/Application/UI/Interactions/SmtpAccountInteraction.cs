using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Interactions;

public class SmtpAccountInteraction : AccountInteractionBase
{
    public SmtpAccount SmtpAccount { get; set; }

    public SmtpAccountInteraction(SmtpAccount smtpAccount, string title)
    {
        SmtpAccount = smtpAccount;
        Title = title;
    }
}
