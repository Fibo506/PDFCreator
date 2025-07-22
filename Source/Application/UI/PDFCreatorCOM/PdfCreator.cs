using System.Runtime.InteropServices;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;

[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("A1F6647E-8C19-4A3E-89DF-7FDFAD2A0C30")]
public interface IPdfCreator
{
    Printers GetPdfCreatorPrinters { get; }
    bool IsInstanceRunning { get; }

    void PrintFile(string path);

    void AddFileToQueue(string path);

    void PrintFileSwitchingPrinters(string path, bool allowDefaultPrinterSwitch);
}

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("69189C58-70C4-4DF2-B94D-5D786E9AD513")]
[ProgId("PDFCreator.PDFCreatorObj")]
public class PdfCreatorObj : IPdfCreator
{
    private readonly BasePdfCreatorObj _basePdfCreatorObj;

    public PdfCreatorObj()
    {
        _basePdfCreatorObj = new BasePdfCreatorObj(new ComDependencyBuilder(new PDFCreatorCOMBootstrapper()));
    }

    public Printers GetPdfCreatorPrinters => new Printers(_basePdfCreatorObj.GetPdfCreatorBasePrinters);
    public bool IsInstanceRunning => _basePdfCreatorObj.IsInstanceRunning;
    public void PrintFile(string path)
    {
        _basePdfCreatorObj.PrintFile(path);
    }

    public void AddFileToQueue(string path)
    {
        _basePdfCreatorObj.AddFileToQueue(path);
    }

    public void PrintFileSwitchingPrinters(string path, bool allowDefaultPrinterSwitch)
    {
        _basePdfCreatorObj.PrintFileSwitchingPrinters(path, allowDefaultPrinterSwitch);
    }
}
