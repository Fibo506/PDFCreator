using pdfforge.Obsidian.Interaction;

namespace pdfforge.PDFCreator.UI.Interactions;

public class OfflineActivationInteraction : IInteraction
{
    public OfflineActivationInteraction(string licenseKey)
    {
        LicenseKey = licenseKey;
        LicenseServerAnswer = "";
    }

    public bool Success { get; set; }

    public string LicenseKey { get; set; }

    public string LicenseServerAnswer { get; set; }
}
