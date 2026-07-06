using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class HotFolderViewTranslation : ITranslatable
{
    public string On { get; private set; } = "ON";
    public string Off { get; private set; } = "OFF";
    public string StartHotFolderWhenWindowsStarts { get; private set; } = "Start HotFolder and standby via autostart of Windows";
    public string ManageHotFolders { get; private set; } = "Manage HotFolders";
    public string NameColumnHeader { get; private set; } = "Printer Name";
    public string PathColumnHeader { get; private set; } = "HotFolder Path";
    public string ProfileColumnHeader { get; private set; } = "Profile";
    public string Active { get; private set; } = "Active";
    public string AddHotFolder { get; private set; } = "Add HotFolder Printer";
    public string ReadyToCreateFirstHotFolder { get; private set; } = "Ready to create your first HotFolder?";

    public string HotFolderDescription { get; private set; } = "HotFolder printers monitor a specified folder and automatically process any files placed in that folder using the selected profile.";

    public string SavingNote { get; private set; } = "Note: All changes will be applied immediately.";

    public string UnlockFolderMonitoring { get; private set; } = "Unlock automatic folder monitoring with HotFolder";

    public string StreamlineYourWorkflow { get; private set; } = "Streamline your workflow with HotFolder.";
    public string OnlyForBusiness { get; private set; } = "This feature is available exclusively in our Business editions.";
    public string BuyALicenseButtonText { get; private set; } = "Upgrade now";

}
