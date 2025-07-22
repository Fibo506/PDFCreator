using System;
using System.Collections.Generic;
using System.Threading;
using NLog;
using pdfforge.PDFCreator.Utilities.Threading;

namespace pdfforge.PDFCreator.Core.ComImplementation;

public class ThreadPool
{
    private readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly Queue<ISynchronizedThread> ThreadQueue = new Queue<ISynchronizedThread>();
    private ISynchronizedThread _conversionThread;
    private bool _isThreadRunning;

    /// <summary>
    ///     Registers to the OnThreadAdded event
    /// </summary>
    public ThreadPool()
    {
        OnThreadAdded += (sender, args) => StartThread();
    }

    public int Count
    {
        get { return ThreadQueue.Count; }
    }

    private event EventHandler<EventArgs> OnThreadAdded;

    /// <summary>
    ///     Adds a new thread to the thread pool
    /// </summary>
    /// <param name="aThread">The thread to be added</param>
    public void AddThread(ISynchronizedThread aThread)
    {
        if (aThread.ThreadState != ThreadState.Unstarted)
            throw new ArgumentException(nameof(aThread));

        Logger.Trace("COM: Pushing new thread into the thread queue.");
        ThreadQueue.Enqueue(aThread);

        if (Count != 1 || _isThreadRunning)
            return;

        Logger.Trace("COM: Thread was added therefore the corresponding event is fired.");
        OnThreadAdded?.Invoke(this, new EventArgs());
    }

    /// <summary>
    ///     Starts the first thread from the thread queue
    /// </summary>
    private void StartThread()
    {
        Logger.Trace("COM: Starting thread...");
        _isThreadRunning = true;
        _conversionThread = ThreadQueue.Dequeue();

        _conversionThread.OnThreadFinished += OnConversionThreadFinished;
        _conversionThread.Start();
    }

    /// <summary>
    ///     Restarts the thread if other threads are available in the queue
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Detailed information about the event</param>
    private void OnConversionThreadFinished(object sender, ThreadFinishedEventArgs e)
    {
        _conversionThread = null;
        _isThreadRunning = false;
        if (Count > 0)
            StartThread();
    }

    /// <summary>
    ///     Waits for each thread to finish
    /// </summary>
    /// <returns>True, if each thread ended within timeOut seconds, false otherwise</returns>
    public void Join()
    {
        while (_conversionThread != null)
        {
            Logger.Trace("Joining the running thread from the thread pool.");
            _conversionThread.Join();
        }
    }

    /// <summary>
    ///     Waits for each thread to finish
    /// </summary>
    /// <param name="timeOut">Amount of time to wait for a each thread</param>
    /// <returns>True, if each thread ended within timeOut, false otherwise</returns>
    public bool Join(TimeSpan timeOut)
    {
        var isJoinSuccessful = true;

        while (_conversionThread != null)
        {
            Logger.Trace("Joining the running thread from the thread pool.");
            isJoinSuccessful &= _conversionThread.Join(timeOut);
        }

        return isJoinSuccessful;
    }
}
