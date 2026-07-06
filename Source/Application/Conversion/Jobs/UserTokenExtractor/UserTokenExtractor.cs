using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.Conversion.Jobs.UserTokenExtractor;

public class UserTokenExtractor : IUserTokenExtractor
{
    private readonly IPdfParserFactory _pdfParserFactory;

    public UserTokenExtractor(IPdfParserFactory pdfParserFactory)
    {
        _pdfParserFactory = pdfParserFactory;
    }

    public ParsedFile ParsePdfFileForUserTokens(string pdfFile, UserTokenSeparator separator)
    {
        var pdfParser = BuildPdfParser(separator);
        var parsedFile = pdfParser.ParseDocument(pdfFile);

        var userToken = new UserToken();
        foreach (var ut in parsedFile.UserToken.KeyValueDict)
            userToken.AddKeyValuePair(ut.Key, ut.Value);

        return new ParsedFile(parsedFile.Filename, userToken, parsedFile.SplitDocument, parsedFile.NumberOfPages);
    }

    private IPdfParser BuildPdfParser(UserTokenSeparator separator)
    {
        {
            string parameterOpenSequence;
            string parameterCloseSequence;

            switch (separator)
            {
                case UserTokenSeparator.AngleBrackets:
                    parameterOpenSequence = "<<<";
                    parameterCloseSequence = ">>>";
                    break;

                case UserTokenSeparator.CurlyBrackets:
                    parameterOpenSequence = "{{{";
                    parameterCloseSequence = "}}}";
                    break;

                case UserTokenSeparator.RoundBrackets:
                    parameterOpenSequence = "(((";
                    parameterCloseSequence = ")))";
                    break;

                case UserTokenSeparator.SquareBrackets:
                default:
                    parameterOpenSequence = "[[[";
                    parameterCloseSequence = "]]]";
                    break;
            }

            return _pdfParserFactory.BuildPdfParser(parameterOpenSequence, parameterCloseSequence);
        }
    }
}
