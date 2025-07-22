using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.ModifyActions.Signature;

/// <summary>
/// Interaction logic for SignaturePasswordOverlayView.xaml
/// </summary>
public partial class SignaturePasswordOverlayView : UserControl
{
    public SignaturePasswordOverlayView(SignaturePasswordOverlayViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
