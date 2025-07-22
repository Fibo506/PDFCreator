using System.Windows;
using System.Windows.Controls;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.TitleReplacementSettings;

class TitleReplacementDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate ReplaceAllDataTemplate { get; set; }
    public DataTemplate ReplaceAtBeginningDataTemplate { get; set; }
    public DataTemplate ReplaceAtEndDataTemplate { get; set; }
    public DataTemplate RegexDataTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var targetItem = item as TitleReplacement;
        if (targetItem == null)
        {
            return null;
        }

        DataTemplate returnValue = null;
        switch (targetItem.ReplacementType)
        {
            case Conversion.Settings.Enums.ReplacementType.Replace:
                returnValue = ReplaceAllDataTemplate;
                break;
            case Conversion.Settings.Enums.ReplacementType.Start:
                returnValue = ReplaceAtBeginningDataTemplate;
                break;
            case Conversion.Settings.Enums.ReplacementType.End:
                returnValue = ReplaceAtEndDataTemplate;
                break;
            case Conversion.Settings.Enums.ReplacementType.RegEx:
                returnValue = RegexDataTemplate;
                break;
        }

        return returnValue;
    }
}
