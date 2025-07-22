using pdfforge.Obsidian.Interaction;

namespace pdfforge.PDFCreator.UI.Interactions;

public abstract class AccountInteractionBase : IInteraction
{
    public virtual bool Success { get; set; }
    public string Title { get; set; }
}
