using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
/// <summary>
/// Interaction logic for FreeEditionHotFolderView.xaml
/// </summary>
public partial class FreeEditionHotFolderView : UserControl
{
    public FreeEditionHotFolderView(FreeEditionHotFolderViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
