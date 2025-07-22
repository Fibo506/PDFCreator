using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;

public partial class OutputFormatPngView : UserControl
{
    public OutputFormatPngView(OutputFormatViewModel viewModel)
    {
        DataContext = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }
}
