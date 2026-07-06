#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using NLog;
using pdfforge.PDFCreator.Core.Controller;
using pdfforge.PDFCreator.Core.Controller.Routing;

namespace pdfforge.PDFCreator.UI.Presentation.Routing;

public class UiLinkHandler : BaseLinkHandler
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IMainWindowThreadLauncher _mainWindowThreadLauncher;
    private readonly IStartupRoutine _startupRoutine;

    public UiLinkHandler(IMainWindowThreadLauncher mainWindowThreadLauncher, IStartupRoutine startupRoutine)
    {
        _mainWindowThreadLauncher = mainWindowThreadLauncher;
        _startupRoutine = startupRoutine;
    }

    public override void HandlePipeLink(string link)
    {
        
    }

    public override void HandleStartupLink(string link)
    {
        if (!TryParseUri(link, out var uri))
        {
            return;
        }

        switch (uri.Host)
        {
            case "mainwindow":
                HandleMainWindowLink(uri);
                break;
            default:
                _logger.Error("Link not handled: {link}", link);
                break;
        }
    }

    private void HandleMainWindowLink(Uri uri)
    {
        switch (uri.Segments)
        {
            case ["/", "tab/", var tab]:
                HandleMainWindowTabLink(tab);
                break;
            default:
                _logger.Error("MainWindow Link not handled: {link}", uri);
                break;
        }

        _mainWindowThreadLauncher.LaunchMainWindow();
    }

    private void HandleMainWindowTabLink(string tab)
    {
        var navigationTarget = tab switch
        {
            _ when tab.Equals(RegionViewName.HomeView, StringComparison.OrdinalIgnoreCase) => RegionViewName.HomeView,
            _ when tab.Equals(RegionViewName.ProfilesView, StringComparison.OrdinalIgnoreCase) => RegionViewName.ProfilesView,
            _ when tab.Equals(RegionViewName.PrinterView, StringComparison.OrdinalIgnoreCase) => RegionViewName.PrinterView,
            _ when tab.Equals(RegionViewName.HotFolderView, StringComparison.OrdinalIgnoreCase) => RegionViewName.HotFolderView,
            _ when tab.Equals(RegionViewName.AccountsView, StringComparison.OrdinalIgnoreCase) => RegionViewName.AccountsView,
            _ => null
        };

        if (navigationTarget == null)
        {
            _logger.Error("MainWindow Tab not found: {tab}", tab);
            return;
        }

        var navigationAction = _startupRoutine.GetActionByType<StartupNavigationAction>().Single();
        navigationAction.Target = navigationTarget;
    }
}
