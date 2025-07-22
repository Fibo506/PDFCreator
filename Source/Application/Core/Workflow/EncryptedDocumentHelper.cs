using System.IO;
using System.Linq;
using NLog;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Core.Workflow;

public interface IEncryptedDocumentHelper
{
    void ValidateEncryptedDocuments(Job job);
}

public class EncryptedDocumentHelper : IEncryptedDocumentHelper
{
    private readonly IPdfProcessor _pdfProcessor;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public EncryptedDocumentHelper(IPdfProcessor pdfProcessor)
    {
        _pdfProcessor = pdfProcessor;
    }

    public void ValidateEncryptedDocuments(Job job)
    {
        var pdfSourceFiles = job.JobInfo.SourceFiles
            .Where(file => Path.GetExtension(file.Filename)?.ToLowerInvariant() == ".pdf")
            .ToList();

        if (!pdfSourceFiles.Any())
        {
            _logger.Trace("No PDF source files found, skipping encryption validation");
            return;
        }

        var hasOwnerPassword = !string.IsNullOrWhiteSpace(job.Passwords?.PdfOwnerPassword);

        foreach (var sourceFileInfo in pdfSourceFiles)
        {
            var pdfFilePath = sourceFileInfo.Filename;
            _logger.Trace("Checking if PDF file is encrypted: {0}", pdfFilePath);

            var pageCount = _pdfProcessor.GetNumberOfPages(pdfFilePath);

            // if the page count is 0 without a password, the PDF is likely encrypted
            if (pageCount <= 0)
            {
                _logger.Debug("PDF file returned 0 pages, likely encrypted: {0}", pdfFilePath);

                var passwordWorked = false;

                if (hasOwnerPassword)
                {
                    _logger.Trace("Trying with owner password for file: {0}", pdfFilePath);
                    pageCount = _pdfProcessor.GetNumberOfPages(pdfFilePath, job.Passwords.PdfOwnerPassword);
                    if (pageCount > 0)
                    {
                        passwordWorked = true;
                        _logger.Trace("Owner password worked for file: {0} (Pages: {1})", pdfFilePath, pageCount);
                    }
                }

                if (!passwordWorked)
                {
                    if (!hasOwnerPassword)
                    {
                        _logger.Error("PDF file is encrypted and no owner password is provided: {0}", pdfFilePath);
                        throw new ProcessingException(
                            $"The PDF file '{Path.GetFileName(pdfFilePath)}' is encrypted and requires a password for processing. Please provide the correct owner password in the security settings.",
                            ErrorCode.Conversion_Ghostscript_PasswordProtectedPDFError);
                    }

                    _logger.Error("PDF file is encrypted and the provided owner password appear to be incorrect: {0}", pdfFilePath);
                    throw new ProcessingException(
                        $"The PDF file '{Path.GetFileName(pdfFilePath)}' is encrypted and the provided owner password appears to be incorrect. Please verify the password in the security settings.",
                        ErrorCode.Conversion_Ghostscript_PasswordProtectedPDFError);
                }
            }
            else
            {
                _logger.Trace("PDF file validation successful: {0} (Pages: {1})", pdfFilePath, pageCount);
            }
        }
    }
}

public class DisabledEncryptedDocumentHelper : IEncryptedDocumentHelper
{
    public void ValidateEncryptedDocuments(Job job)
    {
    }
}
