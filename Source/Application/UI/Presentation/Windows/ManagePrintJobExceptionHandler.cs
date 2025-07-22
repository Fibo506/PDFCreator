using pdfforge.Obsidian;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.UI.Interactions;

namespace pdfforge.PDFCreator.UI.ViewModels;

public class ManagePrintJobExceptionHandler : IManagePrintJobExceptionHandler
{
    private readonly IInteractionInvoker _interactionInvoker;

    public ManagePrintJobExceptionHandler(IInteractionInvoker interactionInvoker)
    {
        _interactionInvoker = interactionInvoker;
    }

    public void HandleException()
    {
        _interactionInvoker.Invoke(new ManagePrintJobsInteraction());
    }
}
