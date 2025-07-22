using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;

/// <summary>
/// Interaction logic for ProgressView.xaml
/// </summary>
public partial class ProgressView : UserControl
{
    public ProgressView(ProgressViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
