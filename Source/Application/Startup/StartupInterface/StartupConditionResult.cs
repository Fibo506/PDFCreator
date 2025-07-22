namespace pdfforge.PDFCreator.Core.StartupInterface;

public class StartupConditionResult
{
    private StartupConditionResult(int exitCode, bool showMessage, string message)
    {
        ExitCode = exitCode;
        ShowMessage = showMessage;
        Message = message;
    }

    public int ExitCode { get; }
    public bool ShowMessage { get; }
    public string Message { get; }
    public bool IsSuccessful => ExitCode == 0;

    public static StartupConditionResult BuildSuccess()
    {
        return new StartupConditionResult(0, false, "");
    }

    public static StartupConditionResult BuildErrorWithMessage(int exitCode, string message, bool showMessage = true)
    {
        return new StartupConditionResult(exitCode, showMessage, message);
    }
}
