using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;

/// <summary>
/// Interaction logic for DropboxShareLinksStepView.xaml
/// </summary>
public partial class DropboxShareLinkStepView : UserControl
{
    public DropboxShareLinkStepView(DropboxShareLinkStepViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
