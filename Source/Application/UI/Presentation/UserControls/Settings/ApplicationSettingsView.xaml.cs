using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings;

/// <summary>
///     Interaction logic for ApplicationSettingsView.xaml
/// </summary>
public partial class ApplicationSettingsView : UserControl
{
    public ApplicationSettingsView(ApplicationSettingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
