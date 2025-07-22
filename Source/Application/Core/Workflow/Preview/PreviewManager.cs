using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Workflow;

public interface IPreviewManager
{
    IList<Task<PreviewPages>> LaunchPreviewTasks(JobInfo jobInfo);
    Task<IList<PreviewPage>> GetTotalPreviewPages(JobInfo jobInfo);
    void AbortAndCleanUpPreview(IList<SourceFileInfo> sourceFileInfos);
    void AbortAndCleanUpPreview(string sfiFilename);
}

public class PreviewManager : IPreviewManager
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private const int MaxConcurrentTasks = 2;
    private readonly SemaphoreSlim _jobSemaphore = new SemaphoreSlim(MaxConcurrentTasks);

    private readonly IPsToPdfConverter _psToPdfConverter;
    private readonly IPdfToPreviewConverter _pdfToPreviewConverter;
    private readonly IDirectory _directory;

    public PreviewManager(IPsToPdfConverter psToPdfConverter, IPdfToPreviewConverter pdfToPreviewConverter, IDirectory directory)
    {
        _psToPdfConverter = psToPdfConverter;
        _pdfToPreviewConverter = pdfToPreviewConverter;
        _directory = directory;
    }

    private readonly Dictionary<string, (Task<PreviewPages> PreviewTask, CancellationTokenSource Cts)> _fileTaskMapping = new();

    public IList<Task<PreviewPages>> LaunchPreviewTasks(JobInfo jobInfo)
    {
        if (jobInfo?.SourceFiles == null)
        {
            _logger.Warn("JobInfo or SourceFiles is null");
            return new List<Task<PreviewPages>>();
        }

        var previewTaskList = new List<Task<PreviewPages>>();

        foreach (var sfi in jobInfo.SourceFiles)
        {
            if (sfi?.Filename == null)
            {
                _logger.Warn("SourceFileInfo or Filename is null, skipping");
                continue;
            }

            try
            {
                var key = GetFileTaskMappingKey(sfi.Filename);
                if (_fileTaskMapping.TryGetValue(key, out var taskCtsTuple))
                {
                    previewTaskList.Add(taskCtsTuple.PreviewTask);
                }
                else
                {
                    var cts = new CancellationTokenSource();
                    var task = Task.Run(() => GeneratePreview(sfi, cts.Token));
                    _fileTaskMapping[key] = (task, cts);
                    previewTaskList.Add(task);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error launching preview task for {0}", sfi.Filename);
                // Add a failed task to maintain consistency
                previewTaskList.Add(Task.FromResult(new PreviewPages(null)));
            }
        }

        return previewTaskList;
    }

    private async Task<PreviewPages> GeneratePreview(SourceFileInfo sfi, CancellationToken cts)
    {
        _logger.Debug("Start generate preview for " + sfi.Filename);

        try
        {
            await _jobSemaphore.WaitAsync(cts);

            if (cts.IsCancellationRequested)
            {
                _logger.Debug("Preview generation cancelled for " + sfi.Filename);
                return new PreviewPages(null);
            }

            try
            {
                _logger.Debug("Generate intermediate pdf for " + sfi.Filename);
                await _psToPdfConverter.ConvertSourceFileToPdf(sfi);

                if (cts.IsCancellationRequested)
                {
                    _logger.Debug("Preview generation cancelled after PDF conversion for " + sfi.Filename);
                    return new PreviewPages(null);
                }

                _logger.Debug("Generate preview images for " + sfi.Filename);
                var preview = await _pdfToPreviewConverter.GeneratePreviewPages(sfi.Filename, cts);

                _logger.Debug("Finished with preview for {0} ({1} pages, {2})",
                    sfi.Filename, preview?.PreviewPageList?.Count ?? 0, preview?.Directory);

                return preview ?? new PreviewPages(null);
            }
            catch (OperationCanceledException)
            {
                _logger.Debug("Preview generation was cancelled for " + sfi.Filename);
                return new PreviewPages(null);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error generating preview for {0}", sfi.Filename);
                return new PreviewPages(null);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.Debug("Preview generation was cancelled while waiting for semaphore for " + sfi.Filename);
            return new PreviewPages(null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error in preview generation for {0}", sfi.Filename);
            return new PreviewPages(null);
        }
        finally
        {
            try
            {
                _jobSemaphore.Release();
            }
            catch (ObjectDisposedException)
            {
                _logger.Debug("SemaphoreSlim was already disposed");
            }
        }
    }

    public async Task<IList<PreviewPage>> GetTotalPreviewPages(JobInfo jobInfo)
    {
        if (jobInfo?.SourceFiles == null)
        {
            _logger.Warn("JobInfo or SourceFiles is null");
            return new List<PreviewPage>();
        }

        try
        {
            var previewTaskList = LaunchPreviewTasks(jobInfo);

            if (!previewTaskList.Any())
            {
                _logger.Debug("No preview tasks to execute");
                return new List<PreviewPage>();
            }

            var previewPagesList = await Task.WhenAll(previewTaskList);
            return AssembleTotalPreviewPages(previewPagesList.ToList());
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error getting total preview pages");
            return new List<PreviewPage>();
        }
    }

    private IList<PreviewPage> AssembleTotalPreviewPages(IList<PreviewPages> previewPagesList)
    {
        if (previewPagesList == null)
            return new List<PreviewPage>();

        var totalPreviewPages = new List<PreviewPage>();
        var pageNumber = 0;

        foreach (var previewPages in previewPagesList)
        {
            if (previewPages?.PreviewPageList == null)
                continue;

            try
            {
                totalPreviewPages.AddRange(previewPages.PreviewPageList.Select(pp =>
                {
                    if (pp != null)
                    {
                        pp.PageNumber = ++pageNumber;
                        pp.SourcePageNumber = pageNumber;
                    }
                    return pp;
                }).Where(pp => pp != null));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error assembling preview pages");
            }
        }

        return totalPreviewPages;
    }

    public void AbortAndCleanUpPreview(IList<SourceFileInfo> sourceFileInfos)
    {
        if (sourceFileInfos == null)
        {
            _logger.Warn("SourceFileInfos is null");
            return;
        }

        foreach (var sfi in sourceFileInfos)
        {
            if (sfi?.Filename != null)
            {
                AbortAndCleanUpPreview(sfi.Filename);
            }
        }
    }

    public void AbortAndCleanUpPreview(string sfiFilename)
    {
        if (string.IsNullOrEmpty(sfiFilename))
        {
            _logger.Warn("Filename is null or empty");
            return;
        }

        try
        {
            var key = GetFileTaskMappingKey(sfiFilename);
            if (_fileTaskMapping.TryGetValue(key, out var taskCtsTuple))
            {
                try
                {
                    taskCtsTuple.Cts.Cancel();
                    _logger.Debug("Cancel preview task for " + sfiFilename);
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex, "Error cancelling task for {0}", sfiFilename);
                }

                try
                {
                    var previewPages = taskCtsTuple.PreviewTask.GetAwaiter().GetResult();

                    if (previewPages?.PreviewPageList != null)
                    {
                        var previewImagePathTaskList = previewPages.PreviewPageList
                            .Where(pp => pp?.PreviewImagePathTask != null)
                            .Select(pp => pp.PreviewImagePathTask);

                        if (previewImagePathTaskList.Any())
                        {
                            Task.WhenAll(previewImagePathTaskList).Wait();
                        }
                    }

                    previewPages?.DisposeDocument?.Invoke();

                    CleanupPreviewDirectory(previewPages?.Directory);
                }
                catch (OperationCanceledException)
                {
                    _logger.Debug("Task was cancelled for " + sfiFilename);
                }
                catch (AggregateException ex)
                {
                    _logger.Error(ex, "Error waiting for task completion and cleanup for {0}", sfiFilename);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error waiting for task completion and cleanup for {0}", sfiFilename);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in AbortAndCleanUpPreview for {0}", sfiFilename);
        }
        finally
        {
            try
            {
                var key = GetFileTaskMappingKey(sfiFilename);
                _fileTaskMapping.Remove(key);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "Error removing task mapping for {0}", sfiFilename);
            }
        }
    }

    private void CleanupPreviewDirectory(string directory)
    {
        if (string.IsNullOrEmpty(directory))
            return;

        try
        {
            if (_directory.Exists(directory))
            {
                _logger.Debug("Deleting preview directory " + directory);
                _directory.Delete(directory, true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.Warn(ex, "Access denied when deleting preview directory: {0}", directory);
        }
        catch (DirectoryNotFoundException)
        {
            _logger.Debug("Preview directory already deleted or doesn't exist: {0}", directory);
        }
        catch (IOException ex)
        {
            _logger.Warn(ex, "IO error when deleting preview directory: {0}", directory);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error deleting preview directory: {0}", directory);
        }
    }

    private string GetFileTaskMappingKey(string sfiFilename)
    {
        try
        {
            return PathSafe.ChangeExtension(sfiFilename, "key");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error creating file task mapping key for {0}", sfiFilename);
            return sfiFilename + ".key"; // Fallback
        }
    }
}
