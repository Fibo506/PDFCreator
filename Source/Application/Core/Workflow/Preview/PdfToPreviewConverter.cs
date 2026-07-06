using System;

using System.Threading;
using System.Threading.Tasks;
using System.Windows.Navigation;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs.FolderProvider;
using pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface.Preview;
using pdfforge.PDFCreator.Utilities;
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
    private readonly IPdfToImagePathTaskList _pdfToImagePathTaskList;

    public PdfToPreviewConverter(IDirectory directory, ITempFolderProvider tempFolderProvider, IGuid guid, IPdfToImagePathTaskList pdfToImagePathTaskList)
    {
        _directory = directory;
        _guid = guid;
        _tempPreviewFolder = PathSafe.Combine(tempFolderProvider.TempFolder, "Preview");
        _pdfToImagePathTaskList= pdfToImagePathTaskList;
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

            var (previewImagePathTaskList, disposeDocument) = _pdfToImagePathTaskList.GetPdfToImagePathList(pdfFilePath, previewImagePathBase, cancellationToken);
            previewPages.DisposeDocument = disposeDocument;

            for (var pageIndex = 0; pageIndex < previewImagePathTaskList.Count; pageIndex++)
            {
                var previewPage = new PreviewPage(pageIndex + 1, previewImagePathTaskList[pageIndex]);
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

    public class DisabledPdfToPreviewConverter : IPdfToPreviewConverter
    {
        public Task<PreviewPages> GeneratePreviewPages(string pdfFilePath, CancellationToken cancellationToken)
        {
            return new Task<PreviewPages>(() => new PreviewPages(""));
        }
    }
}
