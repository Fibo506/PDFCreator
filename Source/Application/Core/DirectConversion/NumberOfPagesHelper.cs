using NLog;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.DirectConversion;

public interface INumberOfPagesHelper
{
    int GetNumberOfPages(string file);
}

public class NumberOfPagesHelper : INumberOfPagesHelper
{
    private readonly IPdfProcessor _pdfProcessor;
    private readonly IFile _file;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public NumberOfPagesHelper(IPdfProcessor pdfProcessor, IFile file)
    {
        _pdfProcessor = pdfProcessor;
        _file = file;
    }

    public int GetNumberOfPages(string file)
    {
        if (DirectConversionHelper.IsPdfFile(file))
            return _pdfProcessor.GetNumberOfPages(file);

        return GetNumberOfPsPages(file);
    }

    private int GetNumberOfPsPages(string filePath)
    {
        var count = 0;
        try
        {
            using (var fs = _file.OpenRead(filePath))
            using (var sr = new System.IO.StreamReader(fs.StreamInstance))
            {
                while (sr.Peek() >= 0)
                {
                    var readLine = sr.ReadLine();
                    if (readLine != null && readLine.Contains("%%Page:"))
                        count++;
                }
            }
        }
        catch
        {
            Logger.Warn("Error while retrieving page count. Set value to 1.");
        }

        return count == 0 ? 1 : count;
    }
}
