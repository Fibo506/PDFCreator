using System.Collections;
using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Core.ComImplementation;
using pdfforge.PDFCreator.Core.JobInfoQueue;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;

[ComVisible(true)]
[Guid("489689FE-E8AF-41FF-8D5A-8212DF2F013C")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface IJobFinishedEvent
{
    void JobFinished();
}

[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("01E51AAE-D371-469A-A556-FC491A81778D")]
public interface IPrintJob
{
    bool IsFinished { get; }
    bool IsSuccessful { get; }

    void SetProfileByGuid(string profileGuid);

    void SetProfileByGuidOrName(string profileGuid);

    OutputFiles GetOutputFiles { get; }

    void ConvertTo(string fullFileName);

    void ConvertToAsync(string fullFileName);

    void SetProfileSetting(string name, string value);

    void SetProfileListSetting(string name, ArrayList value);

    PrintJobInfo PrintJobInfo { get; }

    string GetProfileSetting(string propertyName);

    ArrayList GetProfileListSetting(string propertyName);

    void AddActionToPosition(string actionSettingsName, int addToPosition);

    void AddAction(string actionSettingsName);

    void RemoveAction(string actionSettingsName);
}

[ComVisible(true)]
[ComSourceInterfaces(typeof(IJobFinishedEvent))]
[Guid("9616B8B3-FE6E-4122-AC93-E46DBD571F87")]
[ClassInterface(ClassInterfaceType.None)]
public class PrintJob : IPrintJob
{
    private BasePrintJob _basePrintJob;

    internal PrintJob(BasePrintJob basePrintJob)
    {
        _basePrintJob = basePrintJob;
    }

    protected internal PrintJob(Job job, IJobInfoQueue comJobInfoQueue, IPrintJobAdapterFactory printJobAdapterFactory)
    {
        _basePrintJob = new BasePrintJob(job, comJobInfoQueue, printJobAdapterFactory);
    }

    internal BasePrintJob BasePrintJob => _basePrintJob;

    public bool IsFinished => _basePrintJob.IsFinished;
    public bool IsSuccessful => _basePrintJob.IsSuccessful;

    public void SetProfileByGuid(string profileGuid)
    {
        _basePrintJob.SetProfileByGuid(profileGuid);
    }

    public void SetProfileByGuidOrName(string profileGuid)
    {
        _basePrintJob.SetProfileByGuidOrName(profileGuid);
    }

    public OutputFiles GetOutputFiles => new(_basePrintJob.GetBaseOutputFiles);
    public PrintJobInfo PrintJobInfo => new(_basePrintJob.BasePrintJobInfo);

    public void ConvertTo(string fullFileName)
    {
        _basePrintJob.ConvertTo(fullFileName);
    }

    public void ConvertToAsync(string fullFileName)
    {
        _basePrintJob.ConvertToAsync(fullFileName);
    }

    public void SetProfileSetting(string name, string value)
    {
        _basePrintJob.SetProfileSetting(name, value);
    }

    public void SetProfileListSetting(string name, ArrayList value)
    {
        _basePrintJob.SetProfileListSetting(name, value);
    }

    public string GetProfileSetting(string propertyName)
    {
        return _basePrintJob.GetProfileSetting(propertyName);
    }

    public ArrayList GetProfileListSetting(string propertyName)
    {
        return _basePrintJob.GetProfileListSetting(propertyName);
    }

    public void AddActionToPosition(string actionSettingsName, int addToPosition)
    {
        _basePrintJob.AddActionToPosition(actionSettingsName, addToPosition);
    }

    public void AddAction(string actionSettingsName)
    {
        _basePrintJob.AddAction(actionSettingsName);
    }

    public void RemoveAction(string actionSettingsName)
    {
        _basePrintJob.RemoveAction(actionSettingsName);
    }
}

