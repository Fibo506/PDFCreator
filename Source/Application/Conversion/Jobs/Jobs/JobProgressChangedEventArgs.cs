using System;

namespace pdfforge.PDFCreator.Conversion.Jobs.Jobs;

public class JobProgressChangedEventArgs : EventArgs
{
    public JobProgressChangedEventArgs(Job job, int progressPercentage)
    {
        Job = job;
        ProgressPercentage = progressPercentage;
    }

    public Job Job { get; private set; }
    public int ProgressPercentage { get; private set; }
}
