using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Printer;

/// <summary>
/// Interaction logic for EditPrinterProfileUserUserControl.xaml
/// </summary>
public partial class EditPrinterProfileUserUserControl : UserControl
{
    public EditPrinterProfileUserUserControl(EditPrinterProfileViewModel dataContext)
    {
        DataContext = dataContext;
        InitializeComponent();
    }
}
