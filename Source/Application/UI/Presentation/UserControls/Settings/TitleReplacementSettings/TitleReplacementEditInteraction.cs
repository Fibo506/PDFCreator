using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.TitleReplacementSettings;

public class TitleReplacementEditInteraction : IInteraction
{
    public TitleReplacementEditInteraction(TitleReplacement replacement)
    {
        Replacement = replacement;
    }

    public TitleReplacement Replacement { get; set; }

    public bool Success { get; set; }
}
