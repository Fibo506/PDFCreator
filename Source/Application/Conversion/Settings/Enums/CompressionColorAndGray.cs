using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum CompressionColorAndGray
{
    [Translation("Automatic")]
    Automatic,

    [Translation("JPEG (Maximum)")]
    JpegMaximum,

    [Translation("JPEG (High)")]
    JpegHigh,

    [Translation("JPEG (Medium)")]
    JpegMedium,

    [Translation("JPEG (Low)")]
    JpegLow,

    [Translation("JPEG (Minimum)")]
    JpegMinimum,

    [Translation("JPEG (Manual)")]
    JpegManual,

    [Translation("ZIP")]
    Zip
}
