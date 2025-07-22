using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum PageView
{

    [Translation("Single page view")]
    OnePage,

    [Translation("Floating page view (one page)")]
    OneColumn,

    [Translation("Double page view (odd pages left)")]
    TwoColumnsOddLeft,

    [Translation("Double page view (odd pages right)")]
    TwoColumnsOddRight,

    [Translation("Floating double page view (odd pages left)")]
    TwoPagesOddLeft,

    [Translation("Floating double page view (odd pages right)")]
    TwoPagesOddRight
}
