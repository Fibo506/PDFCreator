using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings.Shared;

public class SettingControlsTranslation : ITranslatable
{
    public string CancelButtonContent { get; private set; } = "Cancel";

    public string SaveButtonContent { get; private set; } = "Save";
}
