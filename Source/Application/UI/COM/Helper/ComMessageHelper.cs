using pdfforge.PDFCreator.Utilities.Messages;

namespace pdfforge.PDFCreator.UI.COM.Helper;

internal class ComMessageHelper : IMessageHelper
{
    public MessageResponse ShowMessage(string message, string title, MessageOptions options, MessageIcon icon, MessageResponse happyPathResponse = MessageResponse.Cancel)
    {
        return happyPathResponse;
    }

    public void ShowHelp(string helpFile, string topic)
    {

    }
}
