using pdfforge.PDFCreator.Core.ComImplementation;

namespace pdfforge.PDFCreator.UI.ComWrapper;

public class ComDependencies
{
    private readonly dynamic _comDependencies;

    internal ComDependencies(dynamic comDependencies)
    {
        _comDependencies = comDependencies;
    }
    public PdfCreatorAdapter PdfCreatorAdapter
    {
        get { return _comDependencies.PdfCreatorAdapter; }
    }

    public QueueAdapter QueueAdapter
    {
        get { return _comDependencies.QueueAdapter; }
    }

}
