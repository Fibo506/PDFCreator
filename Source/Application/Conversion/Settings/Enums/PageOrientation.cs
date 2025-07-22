using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum PageOrientation
{
    [Translation("Auto-Detect")]
    Automatic,
    [Translation("Portrait")]
    Portrait,
    [Translation("Landscape")]
    Landscape
}
