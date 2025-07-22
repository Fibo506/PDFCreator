using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Interactions;

public class FtpAccountInteraction : AccountInteractionBase
{
    public FtpAccount FtpAccount { get; set; }

    public FtpAccountInteraction(FtpAccount ftpAccount, string title)
    {
        FtpAccount = ftpAccount;
        Title = title;
    }
}
