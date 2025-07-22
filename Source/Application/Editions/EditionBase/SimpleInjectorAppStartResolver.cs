using pdfforge.PDFCreator.Core.Startup;
using pdfforge.PDFCreator.Core.StartupInterface;
using SimpleInjector;

namespace pdfforge.PDFCreator.Editions.EditionBase;

internal class SimpleInjectorAppStartResolver : IAppStartResolver
{
    private readonly Container _container;

    public SimpleInjectorAppStartResolver(Container container)
    {
        _container = container;
    }

    public T ResolveAppStart<T>() where T : IAppStart
    {
        return (T)_container.GetInstance(typeof(T));
    }
}
