using System;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs.FolderProvider;
using pdfforge.PDFCreator.Utilities;
using PdfiumViewer;
using SystemInterface.IO;
using Logger = NLog.Logger;

namespace pdfforge.PDFCreator.Core.Workflow;

public interface IPdfToPreviewConverter
{
    Task<PreviewPages> GeneratePreviewPages(string pdfFilePath, CancellationToken cancellationToken);
}

public class PdfToPreviewConverter : IPdfToPreviewConverter
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private const int MaxImageSize = 92; // from PreviewControl


    private readonly IDirectory _directory;
    private readonly IGuid _guid;

    private readonly string _tempPreviewFolder;


    public PdfToPreviewConverter(IDirectory directory, ITempFolderProvider tempFolderProvider, IGuid guid)
    {
        _directory = directory;
        _guid = guid;
        _tempPreviewFolder = PathSafe.Combine(tempFolderProvider.TempFolder, "Preview");
    }

    public async Task<PreviewPages> GeneratePreviewPages(string pdfFilePath, CancellationToken cancellationToken)
    {
        return await Task.Run(() => DoGeneratePreviewPages(pdfFilePath, cancellationToken));
    }

    private PreviewPages DoGeneratePreviewPages(string pdfFilePath, CancellationToken cancellationToken)
    {
        var previewDirectory = PathSafe.Combine(_tempPreviewFolder, _guid.NewGuidString());
        var previewPages = new PreviewPages(previewDirectory);
        var sourceFileNameWithoutExtension = PathSafe.GetFileNameWithoutExtension(pdfFilePath);
        var previewImagePathBase = PathSafe.Combine(previewDirectory, sourceFileNameWithoutExtension);

        try
        {
            _directory.CreateDirectory(previewDirectory);

            if (cancellationToken.IsCancellationRequested)
                return previewPages;

            var document = PdfDocument.Load(pdfFilePath);
            previewPages.DisposeDocument = () => document.Dispose();

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

                var previewPage = new PreviewPage(pageIndex + 1, imagePathTask);
                previewPages.PreviewPageList.Add(previewPage);
            }
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            _logger.Error(ex, "Could not create preview for " + pdfFilePath);
            previewPages.PreviewPageList.Clear();
        }

        return previewPages;
    }
}
