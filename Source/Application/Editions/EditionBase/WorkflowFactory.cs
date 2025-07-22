using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.UI.Presentation.Workflow;
using SimpleInjector;

namespace pdfforge.PDFCreator.Editions.EditionBase;

internal class WorkflowFactory : IWorkflowFactory
{
    private readonly Container _container;

    public WorkflowFactory(Container container)
    {
        _container = container;
    }

    public IConversionWorkflow CreateWorkflow(WorkflowModeEnum mode)
    {
        if (mode == WorkflowModeEnum.Interactive)
            return _container.GetInstance<InteractiveWorkflow>();

        return _container.GetInstance<AutoSaveWorkflow>();
    }
}
