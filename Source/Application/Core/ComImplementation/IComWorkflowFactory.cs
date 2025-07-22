using pdfforge.PDFCreator.Core.Workflow;

namespace pdfforge.PDFCreator.Core.ComImplementation;

public interface IComWorkflowFactory : IWorkflowFactory
{
    IConversionWorkflow BuildWorkflow(string targetFileName);
}
