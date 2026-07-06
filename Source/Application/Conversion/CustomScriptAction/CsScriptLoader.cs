using NLog;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Utilities;
using SystemInterface.IO;

namespace pdfforge.CustomScriptAction;

public class CsScriptLoader : ICustomScriptLoader
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IFile _file;

    public static string CsScriptsFolderName = "CS-Scripts";
    public string ScriptFolder { get; }

    public CsScriptLoader(IFile file, IProgramDataDirectoryHelper programDataDirectoryHelper, IAssemblyHelper assemblyHelper)
    {
        _file = file;
    }

    public LoadScriptResult ReLoadScriptWithValidation(string scriptFile)
    {
        var result = LoadScriptWithValidationInternal(scriptFile, false);
        return result;
    }


    public LoadScriptResult LoadScriptWithValidation(string scriptFilename, bool enableDebugging = false) => new LoadScriptResult(new ActionResult(ErrorCode.CustomScript_ErrorDuringCompilation), null, "");

    private LoadScriptResult LoadScriptWithValidationInternal(string scriptFilename, bool withCaching = true, bool enableDebugging = false)
    {
        return new LoadScriptResult(new ActionResult(ErrorCode.CustomScript_ErrorDuringCompilation), null, "");
    }
}