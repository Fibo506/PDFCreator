using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface.Preview;

public interface IPdfToImagePathTaskList
{
    (IList<Task<string>> ImagePathList, Action DisposeDocument) GetPdfToImagePathList(string pdfFilePath, string previewImagePathBase, CancellationToken cancellationToken);
}
