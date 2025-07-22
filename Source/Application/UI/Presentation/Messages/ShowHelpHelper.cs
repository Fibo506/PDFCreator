using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Web.WebView2.Core;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.Utilities.Messages;
using pdfforge.PDFCreator.Utilities.Web;
using Prism.Events;
using SystemInterface.IO;
using WebViewer;
using Exception = System.Exception;

namespace pdfforge.PDFCreator.UI.Presentation.Messages;

public class ShowHelpHelper : IShowHelpHelper
{
    private readonly IWebLinkLauncher _webLinkLauncher;
    private WebViewerMainWindow _window;

    public ShowHelpHelper(IWebLinkLauncher webLinkLauncher, IEventAggregator eventAggregator)
    {
        _webLinkLauncher = webLinkLauncher;
        eventAggregator.GetEvent<ApplicationClosedEvent>().Subscribe(CloseHelpWindow);
    }

    public void CloseHelpWindow()
    {
        _window = null;
    }

    public void ShowHelp(string helpFolder, string topic, Func<string> getLanguage, Action<string> setLanguage)
    {
        var info = GetInfo();
        var url = PathSafe.Combine(helpFolder, getLanguage(), topic);
        if (info.InstallType == InstallType.NotInstalled)
        {
            _webLinkLauncher.Launch(url);
            return;
        }

        if (_window?.Dispatcher.Thread.IsAlive == false)
        {
            _window = null;
        }

        if (_window == null)
        {
            _window = new WebViewerMainWindow(helpFolder, topic, _webLinkLauncher, getLanguage, setLanguage);
            _window.Closed += (sender, args) => _window = null;
            _window.Show();
        }
        else
        {
            _window.Dispatcher.BeginInvoke(() =>
            {
                _window.Navigate(helpFolder, topic);
                _window.Activate();
                _window.Topmost = true;
                _window.Topmost = false;
                _window.Focus();
            });
        }

    }

    public static InstallInfo GetInfo()
    {
        var version = GetWebView2Version();

        return new InstallInfo(version);
    }
    private static string GetWebView2Version()
    {
        try
        {
            return CoreWebView2Environment.GetAvailableBrowserVersionString();
        }
        catch (Exception) { return ""; }
    }


    public class InstallInfo
    {
        public InstallInfo(string version) => (Version) = (version);

        public string Version { get; }

        public InstallType InstallType => Version switch
        {
            var version when version.Contains("dev") => InstallType.EdgeChromiumDev,
            var version when version.Contains("beta") => InstallType.EdgeChromiumBeta,
            var version when version.Contains("canary") => InstallType.EdgeChromiumCanary,
            var version when !string.IsNullOrEmpty(version) => InstallType.WebView2,
            _ => InstallType.NotInstalled
        };
    }

    public enum InstallType
    {
        WebView2, EdgeChromiumBeta, EdgeChromiumCanary, EdgeChromiumDev, NotInstalled
    }
}
