using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace pdfforge.PDFCreator.Core.Controller.Routing;
public abstract class BaseLinkHandler:ILinkHandler
{
    public abstract void HandlePipeLink(string link);

    public abstract void HandleStartupLink(string link);

    public static (string Action, (string Key, string Value)[] Parameters)? ParseQueryString(string link)
    {
        if (!TryParseUri(link, out var uri))
        {
            return null;
        }

        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var action = queryParams.Get("action");
        if (action == null)
        {
            return null;
        }
        return (action, queryParams.AllKeys.Where(k => k != "action").Select(key => (key!, queryParams.Get(key)!)).ToArray());
    }

    protected static bool TryParseUri(string link, [NotNullWhen(true)] out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrEmpty(link))
        {
            return false;
        }

        try
        {
            var linkUri = new Uri(link);

            if (linkUri.Scheme != "pdfcreator")
            {
                return false;
            }

            uri = linkUri;
            return true;
        }
        catch (UriFormatException)
        {
            return false;
        }
    }
}
