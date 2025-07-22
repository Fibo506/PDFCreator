using System.Windows.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.UI.Presentation.Windows;

namespace pdfforge.PDFCreator.UI.Views.Windows;

public partial class UpdateDownloadWindow : UserControl
{
    public UpdateDownloadWindow(UpdateDownloadWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        TransposerHelper.Register(this, viewModel);
    }
}
