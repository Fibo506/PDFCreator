using System.Collections.ObjectModel;
using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Printer;

public class EditPrinterProfileUserInteraction : IInteraction
{
    public readonly PrinterMappingWrapper PrinterMappingWrapper;
    public ConversionProfileWrapper ResultProfile;
    public ObservableCollection<ConversionProfileWrapper> ProfileWrappers { get; set; }
    public bool Success = false;

    public EditPrinterProfileUserInteraction(PrinterMappingWrapper printerMappingWrapper, ObservableCollection<ConversionProfileWrapper> profiles)
    {
        PrinterMappingWrapper = printerMappingWrapper;
        ProfileWrappers = profiles;
    }

}
