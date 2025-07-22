using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using SystemInterface.IO;
using IWebLinkLauncher = pdfforge.PDFCreator.Utilities.Web.IWebLinkLauncher;

#pragma warning disable CA1416
namespace WebViewer;

/// <summary>
/// Interaction logic for WebViewerMainWindow.xaml
/// </summary>
public partial class WebViewerMainWindow : Window
{
    private readonly string _baseFolder;
    private readonly string _initialHelpTopic;
    private readonly IWebLinkLauncher _webLinkLauncher;
    private readonly Func<string> _getLanguage;
    private readonly Action<string> _setLanguage;
    private CoreWebView2? _webViewCoreWebView2;

    public WebViewerMainWindow(string baseFolder, string initialHelpTopic, IWebLinkLauncher webLinkLauncher, Func<string> getLanguage, Action<string> setLanguage)
    {
        _baseFolder = Directory.CreateDirectory(baseFolder).FullName;
        _initialHelpTopic = initialHelpTopic;
        _webLinkLauncher = webLinkLauncher;
        _getLanguage = getLanguage;
        _setLanguage = setLanguage;

        InitializeComponent();
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        var userDataFolder = Path.GetTempPath();
        var webView2Environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, new CoreWebView2EnvironmentOptions());
        
        await webView.EnsureCoreWebView2Async(webView2Environment);

        _webViewCoreWebView2 = webView.CoreWebView2;
        _webViewCoreWebView2.NavigationStarting += WebViewCoreWebView2OnNavigationStarting;
        _webViewCoreWebView2.Navigate(BuildUrl(_baseFolder, _getLanguage(), _initialHelpTopic));
        
    }

    private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_webViewCoreWebView2 != null)
            _webViewCoreWebView2.NavigationStarting -= WebViewCoreWebView2OnNavigationStarting;
    }

    private void WebViewCoreWebView2OnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        var currentUri = Uri.UnescapeDataString(e.Uri);
        currentUri = currentUri.Contains(@"file:///") ? currentUri[@"file:///".Length..] : currentUri;

        var fileInfo = new FileInfo(currentUri);

        // stop loading page when fileInfo is broken
        if (fileInfo.Directory == null)
        {
            e.Cancel = true;
            return;
        }

        // is it one of our help sites?
        var fileDir = fileInfo.Directory.FullName + "\\";
        if (fileDir.StartsWith(_baseFolder))
        {
            UpdateLanguageIfSwitched(currentUri);
            return;
        }

        // can't find help side, open it in default browser
        e.Cancel = true;
        _webLinkLauncher.Launch(e.Uri);
    }

    private void UpdateLanguageIfSwitched(string currentUri)
    {
        var launcherLanguage = _getLanguage();
        var currentLanguage = ExtractCurrentLanguage(currentUri);

        if (currentLanguage != launcherLanguage)
        {
            _setLanguage(currentLanguage);
        }
    }

    private string ExtractCurrentLanguage(string currentUri)
    {
        var path = currentUri.Substring(_baseFolder.Length)
            .TrimStart('\\', '/');

        var segments = path.Split('\\', '/');

        if (!segments.Any())
            return "en";

        return segments.First();
    }

    public void Navigate(string helpFolder, string topic)
    {
        _webViewCoreWebView2?.Navigate(BuildUrl(helpFolder, _getLanguage(), topic));
    }

    private string BuildUrl(string helpFolder, string language, string topic)
    {
        return PathSafe.Combine(helpFolder, language, topic);
    }

    private void WebViewerMainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        if (_webViewCoreWebView2 != null)
        {
            _webViewCoreWebView2.NavigationStarting -= WebViewCoreWebView2OnNavigationStarting;
            webView.Stop();
            webView.Dispose();
        }
    }
}
#pragma warning restore CA1416
