using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Printer;

public partial class PrinterView : UserControl
{
    public PrinterView(PrinterViewModel viewModel)
    {
        DataContext = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }
}
