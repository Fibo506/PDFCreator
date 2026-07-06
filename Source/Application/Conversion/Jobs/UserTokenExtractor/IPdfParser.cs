namespace pdfforge.PDFCreator.Conversion.Jobs.UserTokenExtractor;

public interface IPdfParser
{
    ParsedFile ParseDocument(string pdfFile);
}

public interface IPdfParserFactory
{
    IPdfParser BuildPdfParser(string parameterOpenSequence, string parameterCloseSequence);
}
