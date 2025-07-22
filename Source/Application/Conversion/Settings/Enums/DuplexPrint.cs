using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum DuplexPrint

{
    [Translation("No duplex printing")]
    Disable,
    [Translation("Long edge turn")]
    LongEdge,
    [Translation("Short edge turn")]
    ShortEdge
}
