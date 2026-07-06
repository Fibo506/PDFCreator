using System.Collections.ObjectModel;
using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class EditHotFolderUserInteraction : IInteraction
{
    public HotFolderConfig HotFolderConfig;
    public PrinterMappingWrapper PrinterMappingWrapper;
    public ConversionProfileWrapper ResultProfile;
    public ObservableCollection<ConversionProfileWrapper> ProfileWrappers { get; set; }

    public bool Success = false;
    public EditHotFolderUserInteraction(HotFolderConfig hotFolderConfig, PrinterMappingWrapper printerMappingWrapper, ObservableCollection<ConversionProfileWrapper> profiles)
    {
        HotFolderConfig = hotFolderConfig;
        PrinterMappingWrapper = printerMappingWrapper;
        ProfileWrappers = profiles;
    }
}
