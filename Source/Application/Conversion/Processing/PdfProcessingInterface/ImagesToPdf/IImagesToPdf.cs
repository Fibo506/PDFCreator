using System.Collections.Generic;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface.ImagesToPdf;

public interface IImagesToPdf
{
    void ConvertImage2Pdf(IList<string> directConversionFiles, ApplicationSettings appSettings, string outputFile);
}
