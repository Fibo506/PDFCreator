namespace pdfforge.PDFCreator.Conversion.Settings;

public partial class PrinterMapping
{
    public PrinterMapping()
    {

    }

    public PrinterMapping(string printerName, string profileGuid)
    {
        PrinterName = printerName;
        ProfileGuid = profileGuid;
    }
}
