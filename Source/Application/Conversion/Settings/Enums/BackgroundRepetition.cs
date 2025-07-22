using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum BackgroundRepetition
{
    [Translation("No repetition")]
    NoRepetition,

    [Translation("Repeat all pages")]
    RepeatAllPages,

    [Translation("Repeat last page")]
    RepeatLastPage
}
