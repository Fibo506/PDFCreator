using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Interactions;

public class HttpAccountInteraction : AccountInteractionBase
{
    public HttpAccount HttpAccount { get; set; }

    public HttpAccountInteraction(HttpAccount httpAccount, string title)
    {
        HttpAccount = httpAccount;
        Title = title;
    }
}
