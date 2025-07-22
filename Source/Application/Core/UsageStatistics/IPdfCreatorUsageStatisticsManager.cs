using System;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Core.UsageStatistics;

public interface IPdfCreatorUsageStatisticsManager
{
    void SendUsageStatistics(TimeSpan duration, Job job, string status);
}
