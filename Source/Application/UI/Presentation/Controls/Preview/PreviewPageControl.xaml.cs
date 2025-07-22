using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.Core.Workflow;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;

/// <summary>
/// Interaction logic for PreviewPageControl.xaml
/// </summary>
public partial class PreviewPageControl : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty IsPreviewLoadingProperty = DependencyProperty.Register(
        nameof(IsPreviewLoading),
        typeof(bool),
        typeof(PreviewPageControl),
        new PropertyMetadata(true));

    public bool IsPreviewLoading
    {
        get { return (bool)GetValue(IsPreviewLoadingProperty); }

        set { SetValue(IsPreviewLoadingProperty, value); }
    }

    public static readonly DependencyProperty PreviewImagePathProperty = DependencyProperty.Register(
        nameof(PreviewImagePath),
        typeof(string),
        typeof(PreviewPageControl),
        new PropertyMetadata(""));

    public string PreviewImagePath
    {
        get { return (string)GetValue(PreviewImagePathProperty); }

        set { SetValue(PreviewImagePathProperty, value); }
    }

    public static readonly DependencyProperty PreviewPageProperty = DependencyProperty.Register(
        nameof(PreviewPage),
        typeof(PreviewPage),
        typeof(PreviewPageControl),
        new PropertyMetadata(null, PreviewPageChangedCallback));

    private static async void PreviewPageChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PreviewPageControl previewPageControl)
        {
            await previewPageControl.Dispatcher.Invoke(async () =>
            {
                if (previewPageControl.PreviewPage == null)
                    return;

                previewPageControl.IsPreviewLoading = true;
                previewPageControl.PreviewImagePath = await previewPageControl.PreviewPage.PreviewImagePathTask;
                previewPageControl.CurrentRotation = previewPageControl.PreviewPage.RotationAngle;
                previewPageControl.IsPreviewLoading = false;
            });
        }
    }

    public PreviewPage PreviewPage
    {
        get { return (PreviewPage)GetValue(PreviewPageProperty); }

        set { SetValue(PreviewPageProperty, value); }
    }

    public static readonly DependencyProperty ShowPageNumberProperty = DependencyProperty.Register(
        nameof(ShowPageNumber),
        typeof(bool),
        typeof(PreviewPageControl),
        new PropertyMetadata(true));

    public bool ShowPageNumber
    {
        get { return (bool)GetValue(ShowPageNumberProperty); }

        set { SetValue(ShowPageNumberProperty, value); }
    }

    public PreviewPageControl()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty ShowPreviewControlsProperty = DependencyProperty.Register(
        nameof(ShowPreviewControls),
        typeof(bool),
        typeof(PreviewPageControl),
        new PropertyMetadata(true));

    public bool ShowPreviewControls
    {
        get { return (bool)GetValue(ShowPreviewControlsProperty); }

        set { SetValue(ShowPreviewControlsProperty, value); }
    }

    public static readonly DependencyProperty CurrentRotationProperty =
        DependencyProperty.Register(
            nameof(CurrentRotation),
            typeof(double),
            typeof(PreviewPageControl),
            new PropertyMetadata(0.0));

    //RotateTransform Angle property is double, that's why this is needed as a double
    public double CurrentRotation
    {
        get { return (double)GetValue(CurrentRotationProperty); }
        set { SetValue(CurrentRotationProperty, value); }
    }

    public ICommand RotatePageCommand => new DelegateCommand(o =>
    {
        if (o is PreviewPageControl previewPageControl)
        {
            previewPageControl.PreviewPage.RotationAngle = (previewPageControl.PreviewPage.RotationAngle + 90) % 360;
            previewPageControl.CurrentRotation = previewPageControl.PreviewPage.RotationAngle;

            OnPropertyChanged(nameof(CurrentRotation));
        }
    });

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
