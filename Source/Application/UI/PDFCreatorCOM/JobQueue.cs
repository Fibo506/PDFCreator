using System.Runtime.InteropServices;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;


[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("3803F46C-F5AA-4B86-8B9C-6EFFAC9CDCFA")]
public interface IJobQueue
{
    void Initialize();

    bool WaitForJob(int timeOut);

    bool WaitForJobs(int jobCount, int timeOut);

    int Count { get; }
    PrintJob NextJob { get; }

    PrintJob GetJobByIndex(int jobIndex);

    void MergeJobs(PrintJob job1, PrintJob job2);

    void MergeAllJobs();

    void Clear();

    void DeleteJob(int index);

    void ReleaseCom();
}

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("66A9CAB1-404A-4918-8DE2-29C26B9B271E")]
[ProgId("PDFCreator.JobQueue")]
public class JobQueue : IJobQueue
{
    private BaseJobQueue _jobQueue;

    public JobQueue()
    {
        _jobQueue = new BaseJobQueue(new ComDependencyBuilder(new PDFCreatorCOMBootstrapper()));
    }

    public void Initialize()
    {
        _jobQueue.Initialize();
    }

    public bool WaitForJob(int timeOut)
    {
        return _jobQueue.WaitForJob(timeOut);
    }

    public bool WaitForJobs(int jobCount, int timeOut)
    {
        return _jobQueue.WaitForJobs(jobCount, timeOut);
    }

    public int Count => _jobQueue.Count;

    public PrintJob NextJob
    {
        get
        {
            BasePrintJob basePrintJob = _jobQueue.NextJob;
            return basePrintJob != null ? new PrintJob(basePrintJob) : null;
        }

    }

    public PrintJob GetJobByIndex(int jobIndex)
    {
        BasePrintJob basePrintJob = _jobQueue.GetJobByIndex(jobIndex);
        return basePrintJob != null ? new PrintJob(basePrintJob) : null;
    }

    public void MergeJobs(PrintJob job1, PrintJob job2)
    {
        _jobQueue.MergeJobs(job1.BasePrintJob, job2.BasePrintJob);
    }

    public void MergeAllJobs()
    {
        _jobQueue.MergeAllJobs();
    }

    public void Clear()
    {
        _jobQueue.Clear();
    }

    public void DeleteJob(int index)
    {
        _jobQueue.DeleteJob(index);
    }

    public void ReleaseCom()
    {
        _jobQueue.ReleaseCom();
    }
}
