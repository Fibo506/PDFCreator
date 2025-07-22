using pdfforge.PDFCreator.Core.StartupInterface;

namespace pdfforge.PDFCreator.Core.Startup;

public interface IAppStartResolver
{
    T ResolveAppStart<T>() where T : IAppStart;
}
