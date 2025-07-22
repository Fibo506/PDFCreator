using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.SendActions.OneDrive;

/// <summary>
/// Interaction logic for OneDriveActionView.xaml
/// </summary>
public partial class OneDriveActionView : UserControl, IActionView
{
    public OneDriveActionView(OneDriveActionViewModel vm)
    {
        DataContext = vm;
        ViewModel = vm;
        TransposerHelper.Register(this, vm);
        InitializeComponent();
    }

    public IActionViewModel ViewModel { get; }
}
