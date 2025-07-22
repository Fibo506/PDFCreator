namespace pdfforge.PDFCreator.Core.Services.Macros;

public class BooleanMacroResult : IMacroResult
{
    public bool Result { get; private set; }

    public BooleanMacroResult(bool result)
    {
        Result = result;
    }

    public void SetResult(bool value)
    {
        Result = value;
    }
}
