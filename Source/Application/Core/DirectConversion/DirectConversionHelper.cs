using System;
using NLog;

namespace pdfforge.PDFCreator.Core.DirectConversion;

public interface IDirectConversionHelper
{
    bool IsDirectConversion(string file);

    bool IsImageConversion(string file);

    bool IsImageOrDirectConversion(string file);
}

public class DirectConversionHelper : IDirectConversionHelper
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public bool IsImageOrDirectConversion(string file)
    {
        return IsDirectConversion(file) || IsImageConversion(file);
    }

    public bool IsDirectConversion(string file)
    {
        return IsPsFile(file) || IsPdfFile(file);
    }

    public bool IsImageConversion(string file)
    {
        return IsImageFile(file);
    }
    private bool IsPsFile(string file)
    {
        return file.EndsWith(".ps", StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool IsPdfFile(string file)
    {
        return file.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase);
    }

    private bool IsImageFile(string file)
    {
        return (file.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase)
               || file.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)
               || file.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase));
    }
}
