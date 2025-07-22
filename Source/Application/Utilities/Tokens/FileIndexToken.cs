namespace pdfforge.PDFCreator.Utilities.Tokens;

public class FileIndexToken : IToken
{
    public string GetValue()
    {
        return "{" + TokenNames.FileIndex + "}";
    }

    public string GetValueWithFormat(string formatString)
    {
        return "{" + TokenNames.FileIndex + "%" + formatString + "}";
    }

    public string GetName()
    {
        return TokenNames.FileIndex;
    }
}
