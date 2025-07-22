using System;

namespace pdfforge.PDFCreator.UI.ComWrapper;

public class PdfCreatorObj
{
    private readonly dynamic _pdfCreatorObj;

    public PdfCreatorObj()
    {
        Type pdfCreatorObjType = Type.GetTypeFromProgID("PDFCreator.PDFCreatorObj");
        _pdfCreatorObj = Activator.CreateInstance(pdfCreatorObjType);
    }
    internal PdfCreatorObj(dynamic pdfCreatorObj)
    {
        _pdfCreatorObj = pdfCreatorObj;
    }
    public Printers GetPdfCreatorPrinters
    {
        get { return new Printers(_pdfCreatorObj.GetPdfCreatorPrinters); }
    }

    public bool IsInstanceRunning
    {
        get { return _pdfCreatorObj.IsInstanceRunning; }
    }

    public void PrintFile(string path)
    {
        _pdfCreatorObj.PrintFile(path);
    }

    public void AddFileToQueue(string path)
    {
        _pdfCreatorObj.AddFileToQueue(path);
    }

    public void PrintFileSwitchingPrinters(string path, bool allowDefaultPrinterSwitch)
    {
        _pdfCreatorObj.PrintFileSwitchingPrinters(path, allowDefaultPrinterSwitch);
    }

}
