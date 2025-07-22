using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum UpdateInterval
{
    [Translation("Daily")]
    Daily,

    [Translation("Weekly")]
    Weekly,

    [Translation("Monthly")]
    Monthly,

    [Translation("Never")]
    Never
}
