using System.Collections.Generic;
using pdfforge.PDFCreator.Core.Printing.Printer;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

public class DesignTimePrinterProvider : IPrinterProvider
{
    public IList<string> GetPDFCreatorPrinters()
    {
        return new[] { "PDFCreator", "PDFCreator2" };
    }
}
