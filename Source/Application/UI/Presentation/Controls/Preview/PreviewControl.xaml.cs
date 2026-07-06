using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;
using pdfforge.PDFCreator.UI.Presentation.Windows;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;

/// <summary>
/// Interaction logic for PDFViewerControl.xaml
/// </summary>
public partial class PreviewControl : UserControl
{
    public PreviewControl()
    {
        InitializeComponent();

        AllowDrop = true;
        Drop += OnDrop;
        DragEnter += OnDragEnter;
        DragOver += OnDragOver;
    }

    private void OnDragEnter(object sender, DragEventArgs e)
    {
        HandleDragEnter(e);
        if (e.Effects != DragDropEffects.None)
        {
            e.Handled = true;
        }
    }

    private void OnDragOver(object sender, DragEventArgs e)
    {
        HandleDragEnter(e); // Same logic as drag enter
        if (e.Effects != DragDropEffects.None)
        {
            e.Handled = true;
        }
    }

    private async void OnDrop(object sender, DragEventArgs e)
    {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length > 0)
        {
            e.Handled = true; // Prevent bubbling to parent
            await HandleMergeDrop(files);
            await ScrollToBottom();
        }
    }

    private async Task ScrollToBottom()
    {
        // Dispatch to UI thread to ensure we're on the correct thread
        await Dispatcher.InvokeAsync(() =>
        {
            if (FindName("PreviewListBox") is ListBox listBox)
            {
                var scrollViewer = GetScrollViewer(listBox);
                scrollViewer?.ScrollToBottom();
            }
        });
    }

    private void HandleDragEnter(DragEventArgs e)
    {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length > 0)
        {
            e.Effects = DragDropEffects.Copy;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
    }

    private async Task HandleMergeDrop(string[] droppedFiles)
    {
        var parentViewModel = FindParentViewModel();
        if (parentViewModel is PrintJobViewModel printJobViewModel)
        {
            await printJobViewModel.MergePreviewDragDrop(droppedFiles);
        }

        // Commenting out as part of PC-5615

        //else
        //{
        //    var parentViewModel = FindParentViewModel();
        //    if (parentViewModel is ManagePrintJobsViewModel managePrintJobsViewModel)
        //    {
        //        await managePrintJobsViewModel.MergePreviewDragDrop(droppedFiles);
        //    }
        //}
    }

    /// <summary>
    /// Walks up the visual tree to find a Window or UserControl with the ViewModel.
    /// Currently only to get the ManagePrintJobsViewModel, but can be expanded for other ViewModels.
    /// </summary>
    private object FindParentViewModel()
    {
        DependencyObject parent = this;
        while (parent != null)
        {
            parent = VisualTreeHelper.GetParent(parent);
            if (parent is FrameworkElement element)
            {
                return element.DataContext switch
                {
                    PrintJobViewModel printJobViewModel => printJobViewModel,
                    ManagePrintJobsViewModel managePrintJobsViewModel => managePrintJobsViewModel,
                    _ => null
                };
            }
        }
        return null;
    }

    private void PreviewListBox_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is ListBox listBox)
        {
            var scrollViewer = GetScrollViewer(listBox);
            if (scrollViewer != null)
            {
                var scrollingUp = e.Delta > 0;
                var scrollingDown = e.Delta < 0;

                var atTop = scrollViewer.VerticalOffset <= 0;
                var atBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;

                if ((scrollingUp && atTop) || (scrollingDown && atBottom))
                {
                    e.Handled = true;

                    DependencyObject parent = listBox;
                    while (parent != null)
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                        if (parent is UIElement uiElement)
                        {
                            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                            {
                                RoutedEvent = UIElement.MouseWheelEvent,
                                Source = sender
                            };
                            uiElement.RaiseEvent(eventArg);
                            break;
                        }
                    }
                }
            }
        }
    }

    private static ScrollViewer? GetScrollViewer(DependencyObject obj)
    {
        if (obj is ScrollViewer viewer)
            return viewer;

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            var result = GetScrollViewer(child);
            if (result != null)
                return result;
        }

        return null;
    }
}
