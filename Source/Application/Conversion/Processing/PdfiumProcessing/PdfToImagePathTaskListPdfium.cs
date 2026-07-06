using System.Drawing.Imaging;
using NLog;
using pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface.Preview;
using PdfiumViewer;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfiumProcessing;

public class PdfToImagePathTaskListPdfium : IPdfToImagePathTaskList
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private const int MaxImageSize = 92; // from PreviewControl
    public (IList<Task<string>> ImagePathList, Action DisposeDocument) GetPdfToImagePathList(string pdfFilePath, string previewImagePathBase, CancellationToken cancellationToken)
    {
        var document = PdfDocument.Load(pdfFilePath);
        var disposeDocument = () => document.Dispose();

        var imagePathList = new List<Task<string>>();

        for (var pageIndex = 0; pageIndex < document.PageCount; pageIndex++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var index = pageIndex;
            var imagePathTask = Task.Run(() =>
            {
                var imagePath = $"{previewImagePathBase}_{index + 1}.jpeg";

                // Determine dimensions based on the page size
                var pageSize = document.PageSizes[index];
                int width, height;
                if (pageSize.Height > pageSize.Width) // Portrait
                {
                    height = MaxImageSize;
                    width = (int)Math.Round(pageSize.Width * MaxImageSize / pageSize.Height);
                }
                else // Landscape
                {
                    width = MaxImageSize;
                    height = (int)Math.Round(pageSize.Height * MaxImageSize / pageSize.Width);
                }

                using var image = document.Render(index, width, height, 96, 96, PdfRenderFlags.Annotations);
                image.Save(imagePath, ImageFormat.Jpeg);

                return imagePath;
            }, cancellationToken);

            imagePathList.Add(imagePathTask);
        }
        return (imagePathList, disposeDocument);
    }
}
