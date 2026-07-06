namespace pdfforge.PDFCreator.Conversion.Settings;

public partial class PrinterMapping
{
    public PrinterMapping()
    {

    }

    public PrinterMapping(string printerName, string profileGuid, bool isHotFolder = false)
    {
        PrinterName = printerName;
        ProfileGuid = profileGuid;
        IsHotFolder = isHotFolder;
    }
}
