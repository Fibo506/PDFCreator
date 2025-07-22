using pdfforge.Obsidian.Interaction;

namespace pdfforge.PDFCreator.UI.Interactions;

public class StoreLicenseForAllUsersInteraction : IInteraction
{
    public StoreLicenseForAllUsersInteraction(string licenseServerCode, string licenseKey)
    {
        LicenseServerCode = licenseServerCode;
        LicenseKey = licenseKey;
    }

    public string LicenseServerCode { get; }
    public string LicenseKey { get; }
}
