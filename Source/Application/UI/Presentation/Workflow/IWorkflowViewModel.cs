using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.UI.Presentation.Workflow;

public interface IWorkflowViewModel
{
    Task ExecuteWorkflowStep(Job job);
}
