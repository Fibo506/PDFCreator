using System.Windows.Controls;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.Shared;

/// <summary>
/// Interaction logic for SaveButtonControl.xaml
/// </summary>
public partial class SaveButtonControl : UserControl
{
    public SaveButtonControl(SettingControlsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
