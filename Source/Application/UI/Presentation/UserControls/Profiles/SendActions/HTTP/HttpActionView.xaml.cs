using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using Prism.Regions;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.SendActions.HTTP;

public partial class HttpActionView : UserControl, IRegionMemberLifetime, IActionView
{
    public bool KeepAlive { get; } = true;

    public HttpActionView(HttpActionViewModel viewModel)
    {
        DataContext = viewModel;
        ViewModel = viewModel;
        TransposerHelper.Register(this, viewModel);
        InitializeComponent();
    }

    public IActionViewModel ViewModel { get; }
}
