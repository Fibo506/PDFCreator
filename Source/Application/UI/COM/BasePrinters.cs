using System;
using pdfforge.PDFCreator.Core.Printing.Printer;

namespace pdfforge.PDFCreator.UI.COM;

public interface IBasePrinters
{
    int Count { get; }

    string GetPrinterByIndex(int index);
}

public class BasePrinters : IBasePrinters
{
    private readonly IPrinterHelper _printerHelper;

    public BasePrinters(IPrinterHelper printerHelper)
    {
        _printerHelper = printerHelper;
    }

    /// <summary>
    ///     Gets the number of actual printer
    /// </summary>
    public int Count
    {
        get { return _printerHelper.GetPDFCreatorPrinters().Count; }
    }

    /// <summary>
    ///     Get the name of the indexed printer of the list
    /// </summary>
    /// <param name="index">Printer position in the printer list</param>
    /// <returns>Name of the printer</returns>
    public string GetPrinterByIndex(int index)
    {
        var printerList = _printerHelper.GetPDFCreatorPrinters();

        if (index >= printerList.Count)
            throw new ArgumentException("Index must not be greater than the actual number of printers available");

        if (index < 0)
            throw new ArgumentException("Index has to be greater or equal to 0");

        return printerList[index];
    }
}
