using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.General;

public partial class HotStandbySettingsView : UserControl
{
    public HotStandbySettingsView(HotStandbySettingsViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        TransposerHelper.Register(this, vm);
    }
}
