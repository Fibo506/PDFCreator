using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Interactions;

public class TimeServerAccountInteraction : AccountInteractionBase
{
    public TimeServerAccount TimeServerAccount { get; set; }

    public TimeServerAccountInteraction(TimeServerAccount timeServerAccount, string title)
    {
        TimeServerAccount = timeServerAccount;
        Title = title;
    }
}
