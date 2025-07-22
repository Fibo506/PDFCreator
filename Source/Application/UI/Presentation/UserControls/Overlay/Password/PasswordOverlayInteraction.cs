using pdfforge.PDFCreator.UI.Interactions;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Overlay.Password;

public class PasswordOverlayInteraction : BasicPasswordOverlayInteraction
{
    public PasswordOverlayInteraction(PasswordMiddleButton middleButtonAction, string title, string passwordDescription, bool showQueryPasswordHint = true)
        : base(middleButtonAction)
    {
        PasswordDescription = passwordDescription;
        MiddleButtonAction = middleButtonAction;
        Title = title;
        ShowQueryPasswordHint = showQueryPasswordHint;
    }

    public string PasswordDescription { get; set; }
    public string Title { get; set; }
    public string IntroText { get; set; } = "";
    public bool ShowQueryPasswordHint { get; set; } = true;
}
