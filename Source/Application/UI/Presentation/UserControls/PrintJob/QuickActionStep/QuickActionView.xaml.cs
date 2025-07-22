using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob.QuickActionStep;

/// <summary>
/// Interaction logic for FtpPasswordView.xaml
/// </summary>
public partial class QuickActionView : UserControl
{
    public QuickActionView(QuickActionViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
