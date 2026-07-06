using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
/// <summary>
/// Interaction logic for EditHotFolderUserControl.xaml
/// </summary>
public partial class EditHotFolderUserControl : UserControl
{
    public EditHotFolderUserControl(EditHotFolderViewModel dataContext)
    {
        DataContext = dataContext;
        TransposerHelper.Register(this, dataContext);
        InitializeComponent();
    }
}
