using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.TitleReplacementSettings;

/// <summary>
///     Interaction logic for TitleReplacementEditUserControl.xaml
/// </summary>
public partial class TitleReplacementEditUserControl : UserControl
{
    public TitleReplacementEditUserControl(TitleReplacementEditUserControlViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
