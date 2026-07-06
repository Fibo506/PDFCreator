using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;


namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public partial class HotFolderView : UserControl
{
    public HotFolderView(HotFolderViewModel viewModel)
    {
        DataContext = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }
}
