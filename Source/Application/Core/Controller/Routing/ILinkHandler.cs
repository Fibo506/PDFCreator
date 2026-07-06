#nullable enable
namespace pdfforge.PDFCreator.Core.Controller.Routing;

public interface ILinkHandler
{
    public void HandlePipeLink(string link);
    public void HandleStartupLink(string link);
}
