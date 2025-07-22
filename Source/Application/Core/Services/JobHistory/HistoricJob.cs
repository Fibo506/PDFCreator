using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.Core.Services.JobHistory;

public class HistoricJob
{
    public IList<HistoricFile> HistoricFiles { get; set; }
    public OutputFormat Format { get; set; }
    public DateTime CreationTime { get; set; }
    public Metadata Metadata { get; set; }
    public int NumberOfPages { get; set; }
    public bool ChangedSinceCreation { get; set; }

    [JsonConstructor]
    public HistoricJob(IList<HistoricFile> historicFiles, OutputFormat format, DateTime creationTime, Metadata metadata, int numberOfPages, bool changedSinceCreation)
    {
        HistoricFiles = historicFiles ?? new List<HistoricFile>();
        Format = format;
        CreationTime = creationTime;
        Metadata = metadata;
        NumberOfPages = numberOfPages;
        ChangedSinceCreation = changedSinceCreation;
    }
}
