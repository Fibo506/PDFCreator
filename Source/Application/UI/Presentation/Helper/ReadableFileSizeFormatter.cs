using System.IO;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public class ReadableReadableFileSizeFormatter : IReadableFileSizeFormatter
{
    public string GetFileSizeString(double speed)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        var order = 0;
        while (speed >= 1024 && order + 1 < sizes.Length)
        {
            order++;
            speed = speed / 1024;
        }

        return $"{speed:0.00} {sizes[order]}";
    }

    public string GetFileSizeString(string FileName)
    {
        return GetFileSizeString(new FileInfo(FileName).Length);
    }
}

public interface IReadableFileSizeFormatter
{
    string GetFileSizeString(double speed);

    string GetFileSizeString(string FileName);
}
