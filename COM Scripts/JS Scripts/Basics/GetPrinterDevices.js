// This script is part of PDFCreator
// License: GPL
// Homepage: https://www.pdfforge.org

var PDFCreator = new ActiveXObject("PDFCreator.PDFCreatorObj");

var printers = PDFCreator.GetPDFCreatorPrinters();
var i = 0;
var allPrinters = "";

if(PDFCreator.IsInstanceRunning)
{
WScript.Echo(PDFCreator.IsInstanceRunning);
}

while(i < printers.Count)
{
    allPrinters += "\n" + printers.GetPrinterByIndex(i);
	i++;
}

WScript.Echo(allPrinters);


