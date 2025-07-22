namespace pdfforge.PDFCreator.Core.Workflow;

public interface IWorkflowFactory
{
    IConversionWorkflow CreateWorkflow(WorkflowModeEnum mode);
}

public enum WorkflowModeEnum
{
    Interactive,
    Autosave
}
