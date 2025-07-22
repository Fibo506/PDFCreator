using System;
using System.Windows;
using MahApps.Metro.Controls;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Core.ServiceLocator;
using pdfforge.PDFCreator.Core.SettingsManagement.Customization;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.UI.Presentation.Workflow;
using pdfforge.PDFCreator.Utilities.Messages;
using Prism.Events;
using Prism.Regions;

namespace pdfforge.PDFCreator.UI.Presentation;

public partial class PrintJobShell : MetroWindow, IWhitelisted
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IDispatcher _dispatcher;
    private readonly IShowHelpHelper _showHelpHelper;
    public InteractiveWorkflowManager InteractiveWorkflowManager { get; }

    public PrintJobShell(IRegionManager regionManager, IInteractiveWorkflowManagerFactory interactiveWorkflowManagerFactory, PrintJobShellViewModel viewModel,
        ICurrentSettingsProvider currentSettingsProvider, ViewCustomization viewCustomization, IEventAggregator eventAggregator, IDispatcher dispatcher, IShowHelpHelper showHelpHelper)
    {
        _eventAggregator = eventAggregator;
        _dispatcher = dispatcher;
        _showHelpHelper = showHelpHelper;
        DataContext = viewModel;
        InitializeComponent();
        InteractiveWorkflowManager = interactiveWorkflowManagerFactory.CreateInteractiveWorkflowManager(regionManager, currentSettingsProvider);
        Closing += (sender, args) => InteractiveWorkflowManager.Cancel = true;

        if (viewCustomization.CustomizationEnabled)
        {
            Title = viewCustomization.PrintJobWindowCaption;
        }
    }

    private void OnTryCloseApplication()
    {
        _dispatcher.BeginInvoke(Close);
    }

    private async void PrintJobShell_OnLoaded(object sender, RoutedEventArgs e)
    {
        _eventAggregator.GetEvent<TryCloseApplicationEvent>().Subscribe(OnTryCloseApplication);
        await InteractiveWorkflowManager.Run();
        await Dispatcher.BeginInvoke(new Action(Close));
    }

    private void PrintJobShell_OnClosed(object sender, EventArgs e)
    {
        _eventAggregator.GetEvent<TryCloseApplicationEvent>().Unsubscribe(OnTryCloseApplication);
        _showHelpHelper.CloseHelpWindow();
    }
}
