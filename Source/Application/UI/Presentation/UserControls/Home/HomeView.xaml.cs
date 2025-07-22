using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Home;

public partial class HomeView : UserControl
{
    public HomeView(HomeViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        TransposerHelper.Register(this, vm);
    }
}
