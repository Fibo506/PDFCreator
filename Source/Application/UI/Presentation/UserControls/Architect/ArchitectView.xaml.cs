using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Architect;

public partial class ArchitectView : UserControl
{
    public ArchitectView(ArchitectViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
