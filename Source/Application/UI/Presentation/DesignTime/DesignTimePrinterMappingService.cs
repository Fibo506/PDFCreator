using System.Collections.Generic;
using System.Collections.ObjectModel;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Services;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;
public class DesignTimePrinterMappingService : IPrinterMappingService
{
    public ObservableCollection<PrinterMappingWrapper> GetPrinterMappings()
    {
        var printerMappings = new Presentation.Helper.SynchronizedCollection<PrinterMappingWrapper>(new List<PrinterMappingWrapper>()).ObservableCollection;
        var profiles = new List<ConversionProfileWrapper>() { new ConversionProfileWrapper(new ConversionProfile()) };
        printerMappings.Add(new PrinterMappingWrapper(new PrinterMapping("PDFCreator", ""), profiles));
        printerMappings.Add(new PrinterMappingWrapper(new PrinterMapping("PDFCreator2", ""), profiles));

        return printerMappings;
    }

    public void Initialize()
    {
    }

    public void AddPrinterMapping(PrinterMappingWrapper newMapping)
    {
    }

    public void RemovePrinterMapping(PrinterMappingWrapper mapping)
    {
    }
}
