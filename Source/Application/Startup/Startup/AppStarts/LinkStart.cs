using pdfforge.PDFCreator.Core.Controller;
using pdfforge.PDFCreator.Core.Controller.Routing;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Threading;

namespace pdfforge.PDFCreator.Core.Startup.AppStarts;

public class LinkStart : MainWindowStart
{
    private readonly IWebLinkHandler _linkHandler;

    public LinkStart(IWebLinkHandler linkHandler, IMaybePipedApplicationStarter maybePipedApplicationStarter, IThreadManager threadManager, IPdfArchitectCheck pdfArchitectCheck, IMainWindowThreadLauncher mainWindowThreadLauncher)
        : base(threadManager, maybePipedApplicationStarter, pdfArchitectCheck, mainWindowThreadLauncher)
    {
        _linkHandler = linkHandler;
    }

    public string Link { get; set; } = "";

    protected override string ComposePipeMessage()
    {
        return $"Link|{Link}";
    }

    protected override bool StartApplication()
    {
        _linkHandler.HandleStartupLink(Link);
        return base.StartApplication();
    }
}
