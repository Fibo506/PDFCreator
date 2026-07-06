using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class EditHotFolderTranslatable : ITranslatable
{
    public string EditHotFolderTitle { get; private set; } = "Edit HotFolder";
    public string LinkedPrinterTitle { get; private set; } = "Linked Printer";
    public string AssignedPrinterName { get; private set; } = "Assigned Printer Name:";
    public string HotFolderTitle { get; private set; } = "HotFolder";
    public string SelectFolderLabel { get; private set; } = "Select a folder to watch:";
    public string SelectWatchFolder { get; private set; } = "Select a folder to watch";
    public string SourceFilesLabel { get; private set; } = "Source files";
    public string SelectSourceFilesFolder { get; private set; } = "Select source files folder";
    public string SourceFilesDescription { get; private set; } = "Select a location the source files should be moved to after they were converted";
    public string SourceFilesDefaultChoiceDescription { get; private set; } = "Move them to the default location (a subfolder of your HotFolder)";
    public string SourceFilesRecycleBinDescription { get; private set; } = "Move them to recycle bin";
    public string SourceFilesCustomDescription { get; private set; } = "Move them to a custom location";
    public string UnprintableFilesLabel { get; private set; } = "Unprintable files";
    public string SelectUnprintableFilesFolder { get; private set; } = "Select unprintable files folder";
    public string UnprintableFilesDescription { get; private set; } = "Select what should happen to files that can’t be printed:";
    public string UnprintableFilesDefaultChoiceDescription { get; private set; } = "Move them to the default location (a subfolder of your HotFolder)";
    public string UnprintableFilesRecycleBinDescription { get; private set; } = "Move them to recycle bin";
    public string UnprintableFilesCustomDescription { get; private set; } = "Move them to a custom location";
    public string FilterLabel { get; private set; } = "Filter";
    public string NoFilter { get; private set; } = "No filter";
    public string ExcludeFilter { get; private set; } = "Exclude all listed extensions from conversion";
    public string RestrictFilter { get; private set; } = "Restrict conversion to filtered file extensions.";
    public string AddFilter { get; private set; } = "Add Filter";
    public string RemoveFilter { get; private set; } = "Remove Filter";
    public string AddFileExtensionFilterTitle { get; private set; } = "Add File Extension Filter";
    public string Profile { get; private set; } = "Profile assigned:";
    public string SelectProfile { get; private set; } = "Select Profile";
    public string Cancel { get; private set; } = "Cancel";
    public string Save { get; private set; } = "Save";
    public string EnterFilterText { get; private set; } = "Enter filter";
    public string FilterNotValid { get; private set; } = "The filter is not valid. Please check your entry.";
}
