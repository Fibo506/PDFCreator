using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.Windows;

public partial class RecommendPdfArchitectView : UserControl
{
    public RecommendPdfArchitectView(RecommendPdfArchitectWindowViewModel viewModel)
    {
        DataContext = viewModel;

        InitializeComponent();
    }
}
