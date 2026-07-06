using System;
using System.Collections.Generic;
using System.Linq;

namespace pdfforge.PDFCreator.Core.Controller.Routing;
public class WebLinkHandler : IWebLinkHandler
{
    private List<ILinkHandler> LinkHandlers { get; }

    public WebLinkHandler(ILinkHandlerProvider linkHandlerProvider)
    {
        LinkHandlers = linkHandlerProvider.GetHandlers();
    }

    public void HandlePipeLink(string link)
    {
        foreach (var linkHandler in LinkHandlers)
        {
            linkHandler.HandlePipeLink(link);
        }
    }

    public void HandleStartupLink(string link)
    {
        foreach (var linkHandler in LinkHandlers)
        {
            linkHandler.HandleStartupLink(link);
        }
    }

    public bool HasHandler(Type type)
    {
        return LinkHandlers.Any(linkHandler => linkHandler.GetType() == type);
    }

    public ILinkHandler GetLinkHandler(Type type)
    {
        return LinkHandlers.FirstOrDefault(linkHandler => linkHandler.GetType() == type);
    }
}

public class InactiveWebLinkHandler : IWebLinkHandler
{
    public void HandlePipeLink(string link)
    {
    }

    public void HandleStartupLink(string link)
    {
    }

    public bool HasHandler(Type type)
    {
        return false;
    }

    public ILinkHandler GetLinkHandler(Type type)
    {
        return null;
    }
}

public interface IWebLinkHandler
{
    void HandlePipeLink(string link);
    void HandleStartupLink(string link);
    bool HasHandler(Type type);
    ILinkHandler GetLinkHandler(Type type);
}
