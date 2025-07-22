using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;

namespace pdfforge.PDFCreator.UI.COM;

public interface IBasePrintJobInfo
{
    string PrintJobName { get; set; }
    string PrintJobAuthor { get; set; }
    string Subject { get; set; }
    string Keywords { get; set; }
}

public class BasePrintJobInfo : IBasePrintJobInfo
{
    private readonly Metadata _metadata;

    public BasePrintJobInfo(Metadata metadata)
    {
        _metadata = metadata;
    }

    /// <summary>
    ///     Title from BasePrintJob
    /// </summary>
    public string PrintJobName
    {
        get { return _metadata.PrintJobName; }
        set { _metadata.PrintJobName = value; }
    }

    /// <summary>
    ///     Author from BasePrintJob
    /// </summary>
    public string PrintJobAuthor
    {
        get { return _metadata.PrintJobAuthor; }
        set { _metadata.PrintJobAuthor = value; }
    }

    /// <summary>
    ///     Subject of the document
    /// </summary>
    public string Subject
    {
        get { return _metadata.Subject; }
        set { _metadata.Subject = value; }
    }

    /// <summary>
    ///     Keywords that describe the document
    /// </summary>
    public string Keywords
    {
        get { return _metadata.Keywords; }
        set { _metadata.Keywords = value; }
    }
}
