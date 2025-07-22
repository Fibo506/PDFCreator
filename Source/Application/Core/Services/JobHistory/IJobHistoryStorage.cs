using System.Collections.Generic;

namespace pdfforge.PDFCreator.Core.Services.JobHistory;

public interface IJobHistoryStorage
{
    List<HistoricJob> Load();

    void Save(IList<HistoricJob> history);
}
