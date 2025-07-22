using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;

public partial class OutputFormatTiffView : UserControl
{
    public OutputFormatTiffView(OutputFormatViewModel viewModel)
    {
        DataContext = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }
}
