using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum CompressionMonochrome
{
    [Translation("CCITT Fax")]
    CcittFaxEncoding,

    [Translation("ZIP Compression")]
    Zip,

    [Translation("Run Length Compression")]
    RunLengthEncoding
}
