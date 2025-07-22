using System.Windows;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.Help;

public class HelpCommandHandler
{
    private readonly IUserGuideHelper _userGuideHelper;

    public HelpCommandHandler(IUserGuideHelper userGuideHelper)
    {
        _userGuideHelper = userGuideHelper;
    }

    public void RegisterHelpCommandBinding()
    {
        var commandBinding = new CommandBinding(ApplicationCommands.Help, ShowHelpExecuted, ShowHelpCanExecute);
        CommandManager.RegisterClassCommandBinding(typeof(FrameworkElement), commandBinding);
    }

    private void ShowHelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        FrameworkElement senderElement = sender as FrameworkElement;

        var helpTopic = HelpProvider.GetHelpTopic(senderElement);
        if (helpTopic != HelpTopic.Unset)
            e.CanExecute = true;
    }

    private void ShowHelpExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        FrameworkElement senderElement = sender as FrameworkElement;
        var helpTopic = HelpProvider.GetHelpTopic(senderElement);
        _userGuideHelper.ShowHelp(helpTopic);
    }
}
