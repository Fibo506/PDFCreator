namespace pdfforge.PDFCreator.UI.Presentation.Windows.Startup;

public class LicenseOptionProvider
{
    public LicenseOptionProvider(bool hideLicenseOptions)
    {
        HideLicenseOptions = hideLicenseOptions;
    }

    public bool HideLicenseOptions { get; }
}
