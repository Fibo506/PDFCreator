namespace pdfforge.PDFCreator.Core.Communication;

public interface IPipeMessageHandler
{
    void HandlePipeMessage(string message);
}
