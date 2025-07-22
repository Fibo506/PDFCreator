using System.Windows;

namespace pdfforge.PDFCreator.UI.Presentation.Help;

public static class HelpProvider
{
    public static readonly DependencyProperty HelpTopicProperty =
        DependencyProperty.RegisterAttached("HelpTopic", typeof(HelpTopic), typeof(HelpProvider), new PropertyMetadata(HelpTopic.Unset));

    public static HelpTopic GetHelpTopic(DependencyObject obj)
    {
        return (HelpTopic)obj.GetValue(HelpTopicProperty);
    }

    public static void SetHelpTopic(DependencyObject obj, HelpTopic value)
    {
        obj.SetValue(HelpTopicProperty, value);
    }
}
