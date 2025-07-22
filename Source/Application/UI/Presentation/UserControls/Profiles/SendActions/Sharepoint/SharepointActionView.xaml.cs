namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles.SendActions.Sharepoint;

/// <summary>
/// Interaction logic for SharepointActionView.xaml
/// </summary>
public partial class SharepointActionView : IActionView
{
    private readonly IActionViewModel _vm;

    public SharepointActionView(SharepointActionViewModel viewModel)
    {
        _vm = viewModel;
        DataContext = viewModel;
        //TransposerHelper.Register(this, _vm);
        InitializeComponent();
    }

    public IActionViewModel ViewModel => _vm;

}
