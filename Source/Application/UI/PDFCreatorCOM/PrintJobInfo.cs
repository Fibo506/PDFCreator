using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;


[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("E64E06AF-E8A1-4585-AE18-A1996836351D")]
public interface IPrintJobInfo
{
    string PrintJobName { get; set; }
    string PrintJobAuthor { get; set; }
    string Subject { get; set; }
    string Keywords { get; set; }
}

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("95648C09-EBFE-4472-ADB7-CAC16ED85029")]
public class PrintJobInfo : IPrintJobInfo
{
    private readonly BasePrintJobInfo _basePrintJobInfo;

    internal PrintJobInfo(BasePrintJobInfo basePrintJobInfo)
    {
        _basePrintJobInfo = basePrintJobInfo;
    }

    protected internal PrintJobInfo(Metadata metadata)
    {
        _basePrintJobInfo = new BasePrintJobInfo(metadata);
    }

    public string PrintJobName
    {
        get => _basePrintJobInfo.PrintJobName;
        set => _basePrintJobInfo.PrintJobName = value;
    }
    public string PrintJobAuthor
    {
        get => _basePrintJobInfo.PrintJobAuthor;
        set => _basePrintJobInfo.PrintJobAuthor = value;
    }
    public string Subject
    {
        get => _basePrintJobInfo.Subject;
        set => _basePrintJobInfo.Subject = value;
    }
    public string Keywords
    {
        get => _basePrintJobInfo.Keywords;
        set => _basePrintJobInfo.Keywords = value;
    }
}

