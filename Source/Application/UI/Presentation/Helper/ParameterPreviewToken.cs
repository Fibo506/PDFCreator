using System;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public class ParameterPreviewToken : IToken
{
    private readonly string _tokenName;
    private readonly Func<string, string> _previewFunc;

    public ParameterPreviewToken(string tokenName, Func<string, string> previewFunc)
    {
        _tokenName = tokenName;
        _previewFunc = previewFunc;
    }

    public string GetValue()
    {
        return "";
    }

    public string GetValueWithFormat(string formatString)
    {
        var formatArray = formatString.Split(new[] { ':' }, 2);
        return _previewFunc(formatArray[0]);
    }

    public string GetName()
    {
        return _tokenName;
    }
}
