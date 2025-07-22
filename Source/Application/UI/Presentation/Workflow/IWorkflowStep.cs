using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.UI.Presentation.Workflow;

public interface IWorkflowStep
{
    string NavigationUri { get; }

    bool IsStepRequired(Job job);

    Task ExecuteStep(Job job, IWorkflowViewModel workflowViewModel);
}
