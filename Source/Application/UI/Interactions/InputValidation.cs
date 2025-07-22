namespace pdfforge.PDFCreator.UI.Interactions;

public class InputValidation
{
    public InputValidation(bool isValid)
    {
        IsValid = isValid;
    }

    public InputValidation(bool isValid, string message)
    {
        IsValid = isValid;
        Message = message;
    }

    public bool IsValid { get; set; }

    public string Message { get; set; }
}
