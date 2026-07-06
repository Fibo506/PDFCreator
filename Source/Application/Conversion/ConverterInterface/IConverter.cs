using System;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Conversion.ConverterInterface;

public interface IConverter
{
    void Init(bool outputFormatIsPdf, bool isIntermediateFileRequired);

    void FirstConversionStep(Job job);

    void SecondConversionStep(Job job);

    void CreateIntermediatePdf(Job job);

    string ConverterOutput { get; }

    event EventHandler<ConversionProgressChangedEventArgs> OnReportProgress;
}

public class ConversionProgressChangedEventArgs : EventArgs
{
    public ConversionProgressChangedEventArgs(int progress)
    {
        Progress = progress;
    }

    public int Progress { get; private set; }
}
