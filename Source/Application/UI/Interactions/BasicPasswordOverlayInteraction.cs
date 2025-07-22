using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.UI.Interactions.Enums;

namespace pdfforge.PDFCreator.UI.Interactions;

public class BasicPasswordOverlayInteraction : IInteraction
{
    public BasicPasswordOverlayInteraction(PasswordMiddleButton middleButtonAction)
    {
        MiddleButtonAction = middleButtonAction;
    }

    public PasswordMiddleButton MiddleButtonAction { get; set; }

    public string Password { get; set; }

    public PasswordResult Result { get; set; } = PasswordResult.Cancel;
}
