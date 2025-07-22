using System.Windows.Controls;
using System.Windows.Input;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Misc;

public partial class MessageView : UserControl
{
    public MessageView(MessageViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        ((MessageViewModel)DataContext).CopyToClipboard_CommandBinding(sender, e);
    }
}
