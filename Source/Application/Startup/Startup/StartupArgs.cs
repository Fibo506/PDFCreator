using pdfforge.PDFCreator.Core.StartupInterface;

namespace pdfforge.PDFCreator.Core.Startup;

public record StartupArgs : IStartupArgs
{
    public string[] Args { get; init; }
}
