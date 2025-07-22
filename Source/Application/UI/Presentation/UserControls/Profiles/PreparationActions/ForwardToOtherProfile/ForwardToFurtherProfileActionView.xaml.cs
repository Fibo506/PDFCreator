using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.PreparationActions.ForwardToOtherProfile;

public partial class ForwardToFurtherProfileActionView : UserControl, IActionView
{
    public ForwardToFurtherProfileActionView(IForwardToFurtherProfileViewModel viewModel)
    {
        DataContext = viewModel;
        ViewModel = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }

    public IActionViewModel ViewModel { get; }
}
