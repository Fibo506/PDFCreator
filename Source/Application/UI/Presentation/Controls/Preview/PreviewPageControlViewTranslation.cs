using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;
public class PreviewPageControlViewTranslation : ITranslatable
{
    public string RemovePage { get; private set; } = "Remove Page";
    public string UndoRemovePage { get; private set; } = "Undo Removal";

    public string RotatePage { get; private set; } = "Rotate Page";
    public string BusinessFeature { get; private set; } = "Business Feature";
}
