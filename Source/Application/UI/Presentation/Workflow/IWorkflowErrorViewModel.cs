using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.UI.Presentation.Workflow;

public interface IWorkflowErrorViewModel
{
    Task ExecuteWorkflowStep(Job job, ActionResult error, bool asWarning);
}
