using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
/// <summary>
/// Interaction logic for AddHotFolderFilterUserControl.xaml
/// </summary>
public partial class AddHotFolderFilterUserControl : UserControl
{
    public AddHotFolderFilterUserControl(AddHotFolderFilterViewModel dataContext)
    {
        DataContext = dataContext;
        InitializeComponent();
    }
}
