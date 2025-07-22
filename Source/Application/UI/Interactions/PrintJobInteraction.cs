using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.UI.Interactions.Enums;

namespace pdfforge.PDFCreator.UI.Interactions;

public class PrintJobInteraction : IInteraction
{
    public PrintJobInteraction(JobInfo jobInfo, ConversionProfile profile)
    {
        JobInfo = jobInfo;
        Profile = profile;
    }

    public JobInfo JobInfo { get; set; }

    public ConversionProfile Profile { get; set; }

    public PrintJobAction PrintJobAction { get; set; } = PrintJobAction.Cancel;
}
