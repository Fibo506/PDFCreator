namespace pdfforge.PDFCreator.Conversion.ActionsInterface;

public record PageMapping(int SourcePageNumber, int PreviewPageNumber, int RotationAngle, bool IsExcluded);
