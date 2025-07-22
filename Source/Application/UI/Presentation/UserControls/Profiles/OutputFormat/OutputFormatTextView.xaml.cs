using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;

public partial class OutputFormatTextView : UserControl
{
    public OutputFormatTextView(OutputFormatViewModel vm)
    {
        DataContext = vm;
        TransposerHelper.Register(this, vm);
        InitializeComponent();
    }
}
