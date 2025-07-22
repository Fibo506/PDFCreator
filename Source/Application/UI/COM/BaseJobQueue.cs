using System;
using NLog;
using pdfforge.PDFCreator.Core.ComImplementation;
using pdfforge.PDFCreator.Core.JobInfoQueue;

namespace pdfforge.PDFCreator.UI.COM;

public interface IBaseQueue
{
    void Initialize();

    bool WaitForJob(int timeOut);

    bool WaitForJobs(int jobCount, int timeOut);

    int Count { get; }
    BasePrintJob NextJob { get; }

    BasePrintJob GetJobByIndex(int jobIndex);

    void MergeJobs(BasePrintJob job1, BasePrintJob job2);

    void MergeAllJobs();

    void Clear();

    void DeleteJob(int index);

    void ReleaseCom();
}

public class BaseJobQueue : IBaseQueue
{
    private readonly ComDependencyBuilder _comDependencyBuilder;
    private IJobInfoQueue _jobInfoQueue;
    private IPrintJobAdapterFactory _printJobAdapterFactory;
    private QueueAdapter _queueAdapter;
    private Logger _logger = LogManager.GetCurrentClassLogger();

    public BaseJobQueue(ComDependencyBuilder comDependencyBuilder)
    {
        _comDependencyBuilder = comDependencyBuilder;
    }

    /// <summary>
    ///     Initializes the essential components like JobInfoQueue for the COM object
    /// </summary>
    public virtual void Initialize()
    {
        try
        {
            var dependencies = _comDependencyBuilder.ComDependencies();

            _queueAdapter = dependencies.QueueAdapter;
            _printJobAdapterFactory = _queueAdapter.PrintJobAdapterFactory;
            _jobInfoQueue = _queueAdapter.JobInfoQueue;

            _queueAdapter.Initialize();
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
    }

    /// <summary>
    ///     Waits for exactly one job to enter the queue
    /// </summary>
    /// <param name="timeOut">Duration which the queue should wait for a job</param>
    /// <returns>False, if the duration was exceeded. Otherwise it returns true</returns>
    public virtual bool WaitForJob(int timeOut)
    {
        return _queueAdapter.WaitForJob(timeOut);
    }

    /// <summary>
    ///     Waits for n jobs to enter the queue
    /// </summary>
    /// <param name="jobCount">Number of jobs to wait for</param>
    /// <param name="timeOut">Duration which the queue should wait for the n jobs</param>
    /// <returns>False, if the duration was exceeded. Otherwise it returns true</returns>
    public bool WaitForJobs(int jobCount, int timeOut)
    {
        return _queueAdapter.WaitForJobs(jobCount, timeOut);
    }

    /// <summary>
    ///     Returns the number of jobs in the queue
    /// </summary>
    public int Count => _queueAdapter.Count;

    /// <summary>
    ///     Returns the next job in the queue as a ComJob
    /// </summary>
    public BasePrintJob NextJob => new BasePrintJob(_queueAdapter.NextJob, _jobInfoQueue, _printJobAdapterFactory);

    /// <summary>
    ///     Creates the job from the queue by index
    /// </summary>
    /// <param name="jobIndex">Index of the jobinfo in the queue</param>
    /// <returns>The corresponding ComJob</returns>
    public BasePrintJob GetJobByIndex(int jobIndex)
    {
        return new BasePrintJob(_queueAdapter.JobById(jobIndex), _jobInfoQueue, _printJobAdapterFactory);
    }

    /// <summary>
    ///     Merges two ComJobs
    /// </summary>
    /// <param name="job1">The first job to merge</param>
    /// <param name="job2">The second job to merge</param>
    public void MergeJobs(BasePrintJob job1, BasePrintJob job2)
    {
        _queueAdapter.MergeJobs(job1.JobInfo, job2.JobInfo);
    }

    /// <summary>
    ///     Merges all jobs in the queue
    /// </summary>
    public void MergeAllJobs()
    {
        _queueAdapter.MergeAllJobs();
    }

    /// <summary>
    ///     Remove all elements from the Queue
    /// </summary>
    public void Clear()
    {
        _queueAdapter.Clear();
    }

    /// <summary>
    ///     Deletes a chosen print job.
    /// </summary>
    /// <param name="index">Determines the print job to be removed by its position in the queue.</param>
    public void DeleteJob(int index)
    {
        _queueAdapter.DeleteJob(index);
    }

    /// <summary>
    ///     Shuts down the used instance
    /// </summary>
    public void ReleaseCom()
    {
        _queueAdapter.ReleaseCom();
    }
}
