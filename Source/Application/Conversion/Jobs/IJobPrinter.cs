using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Conversion.Jobs;

public interface IJobPrinter
{
    void Print(Job job);
}
