using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Core.ServiceLocator;
using pdfforge.PDFCreator.Core.Services.Macros;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.Utilities.Update;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation;

public partial class MainShell : MetroWindow, IWhitelisted
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IDispatcher _dispatcher;
    private bool _skipSettingsCheck;
    public IUpdateHelper UpdateHelper { get; }

    public MainShellViewModel ViewModel => (MainShellViewModel)DataContext;

    public MainShell(MainShellViewModel vm, IUpdateHelper updateHelper, IEventAggregator eventAggregator, IDispatcher dispatcher)
    {
        _eventAggregator = eventAggregator;
        _dispatcher = dispatcher;
        DataContext = vm;
        UpdateHelper = updateHelper;
        InitializeComponent();
        vm.Init(Close);
        TransposerHelper.Register(this, vm);
    }

    private void OnTryCloseApplicationEvent()
    {
        _dispatcher.BeginInvoke(Close);
    }

    protected override void OnClosed(EventArgs e)
    {
        ViewModel.OnClosed();
        _eventAggregator.GetEvent<TryCloseApplicationEvent>().Unsubscribe(OnTryCloseApplicationEvent);
        base.OnClosed(e);
    }

    public override void EndInit()
    {
        base.EndInit();
        (DataContext as MainShellViewModel)?.PublishMainShellDone();
    }

    private void MainShell_OnLoaded(object sender, RoutedEventArgs e)
    {
        // create a window of the size of the screen to get its dpi
        var window = new Window();
        window.Show();
        var source = PresentationSource.FromVisual(window);
        double dpiX = 1, dpiY = 1;
        if (source != null)
        {
            // Get the DPI of the screen
            dpiX = source.CompositionTarget.TransformToDevice.M11;
            dpiY = source.CompositionTarget.TransformToDevice.M22;
        }
        window.Close();

        // Get the size of the screen
        var scaledWidth = dpiX * SystemParameters.PrimaryScreenWidth;
        var scaledHeight = dpiY * SystemParameters.PrimaryScreenHeight;

        // if the screen is smaller than the window, maximize the window
        if (scaledWidth < Width || scaledHeight < Height)
            WindowState = WindowState.Maximized;

        _eventAggregator.GetEvent<TryCloseApplicationEvent>().Subscribe(OnTryCloseApplicationEvent);
        FocusManager.SetFocusedElement(this, HomeButton);
    }

    private async void MainShell_OnClosing(object sender, CancelEventArgs e)
    {
        if (_skipSettingsCheck)
        {
            return;
        }

        e.Cancel = true;
        _skipSettingsCheck = false;
        var result = await ViewModel.CloseCommand.ExecuteAsync(ViewModel.SettingsLoading);

        if (result == ResponseStatus.Success)
        {
            _skipSettingsCheck = true;

            _eventAggregator.GetEvent<ApplicationClosedEvent>().Publish();
            // Invoke required because we can't call Close during the closing event
            await Dispatcher.BeginInvoke(Close);
        }
    }
}
