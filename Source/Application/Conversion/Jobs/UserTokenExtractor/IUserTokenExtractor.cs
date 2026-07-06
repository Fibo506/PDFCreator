using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.Conversion.Jobs.UserTokenExtractor;

public interface IUserTokenExtractor
{
    ParsedFile ParsePdfFileForUserTokens(string pdfFile, UserTokenSeparator separator);
}
