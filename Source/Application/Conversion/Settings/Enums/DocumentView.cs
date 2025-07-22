using Translatable;

namespace pdfforge.PDFCreator.Conversion.Settings.Enums;

[Translatable]
public enum DocumentView
{
    [Translation("Neither document outline nor thumbnail images visible")]
    NoOutLineNoThumbnailImages,

    [Translation("Document outline visible")]
    Outline,

    [Translation("Thumbnail images visible")]
    ThumbnailImages,

    [Translation("Fullscreen mode")]
    FullScreen,

    [Translation("Optional content group panel visible")]
    ContentGroupPanel,

    [Translation("Attachments panel visible")]
    AttachmentsPanel
}
