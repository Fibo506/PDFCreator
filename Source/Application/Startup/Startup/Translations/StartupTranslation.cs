using Translatable;

namespace pdfforge.PDFCreator.Core.Startup.Translations;

public class StartupTranslation : ITranslatable
{
    public string NoSupportedGSFound { get; private set; } = "Can't find a supported Ghostscript installation.\n\nProgram exiting now.";
    public string SpoolerNotRunning { get; private set; } = "The Windows spooler service is not running. Please start the spooler first.\n\nProgram exiting now.";
}
