using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum ColorModel
{
    [Translation("RGB")]
    Rgb,
    [Translation("CMYK")]
    Cmyk,
    [Translation("Grayscale")]
    Gray
}
