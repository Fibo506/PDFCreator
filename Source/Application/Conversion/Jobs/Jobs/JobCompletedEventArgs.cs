using System;

namespace pdfforge.PDFCreator.Conversion.Jobs.Jobs;

public class JobCompletedEventArgs : EventArgs
{
    public JobCompletedEventArgs(Job job)
    {
        Job = job;
    }

    public Job Job { get; private set; }
}
