using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace pdfforge.PDFCreator.Core.Controller.Routing;

public interface ILinkHandlerProvider
{
    List<ILinkHandler> GetHandlers();
}

public class LinkHandlerProvider : ILinkHandlerProvider
{
    private List<ILinkHandler> _handlers = [];

    public List<ILinkHandler> GetHandlers()
    {
        return _handlers;
    }

    public void AddHandlers(List<ILinkHandler> list)
    {
        _handlers = list;
    }
}
