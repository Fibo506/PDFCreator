using System;
using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Core.UsageStatistics;

public interface IUsageStatisticsManager
{
    Task SendUsageStatistics(TimeSpan duration, Job job, string status);

    Task SendServiceStatistics(TimeSpan duration);

    bool EnableUsageStatistics { get; set; }
}
