using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Core.Printing.Printer;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;

[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("CC64BF81-0A5C-4EC9-9EF4-31DFF3ABD92C")]
public interface IPrinters
{
    int Count { get; }
    string GetPrinterByIndex(int index);
}

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("267F0F22-4C1B-4B36-AA67-D6F2EB9C2423")]
public class Printers : IPrinters
{
    private readonly BasePrinters _basePrinters;

    internal Printers(BasePrinters basePrinters)
    {
        _basePrinters = basePrinters;
    }

    protected internal Printers(IPrinterHelper printerHelper)
    {
        _basePrinters = new BasePrinters(printerHelper);
    }

    public int Count => _basePrinters.Count;
    public string GetPrinterByIndex(int index)
    {
        return _basePrinters.GetPrinterByIndex(index);
    }
}

