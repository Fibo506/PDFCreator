using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs.FolderProvider;
using pdfforge.PDFCreator.Utilities;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Services.Update;

public class UpdateDownloader : IUpdateDownloader
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IDirectory _directory;
    private readonly IFile _systemFile;
    private readonly ITempFolderProvider _tempFolderProvider;
    private readonly IHashUtil _hashUtil;
    private readonly ICancellationTokenSourceFactory _cancellationSourceFactory;

    private readonly HttpClient _httpClient = new HttpClient();
    public DownloadSpeed DownloadSpeed { get; set; }

    public event EventHandler<UpdateProgressChangedEventArgs> OnDownloadFinished;

    public event EventHandler<UpdateProgressChangedEventArgs> OnProgressChanged;

    private Task _downloadTask;
    private CancellationTokenSource _cancellationSource;

    public UpdateDownloader(IDirectory directory, IFile systemFile, ITempFolderProvider tempFolderProvider, IHashUtil hashUtil, ICancellationTokenSourceFactory cancellationSourceFactory)
    {
        _directory = directory;
        _systemFile = systemFile;
        _tempFolderProvider = tempFolderProvider;
        _hashUtil = hashUtil;
        _cancellationSourceFactory = cancellationSourceFactory;
    }

    public string GetDownloadPath(string downloadUrl)
    {
        var downloadLocation = _tempFolderProvider.TempFolder;
        _directory.CreateDirectory(downloadLocation);
        var uri = new Uri(downloadUrl);
        var filename = PathSafe.GetFileName(uri.LocalPath);
        return PathSafe.Combine(downloadLocation, filename);
    }

    public async Task StartDownloadAsync(IApplicationVersion version)
    {
        if (_downloadTask == null)
        {
            _cancellationSource = _cancellationSourceFactory.CreateSource();
            _downloadTask = Task.Run(async () =>
            {
                DownloadSpeed = new DownloadSpeed();

                OnProgressChanged += DownloadSpeed.DownloadProgressChanged;
                OnDownloadFinished += DownloadSpeed.DownloadFileCompleted;

                var downloadFileWithRange = await DownloadFileWithRange(version, _cancellationSource.Token);
                OnDownloadFinished?.Invoke(this, new UpdateProgressChangedEventArgs(downloadFileWithRange, 0, 0, 0));
                _downloadTask = null;
            }, _cancellationSource.Token);
        }

        await _downloadTask;
    }

    public bool IsDownloaded(string filePath)
    {
        // file is already downloaded
        if (_systemFile.Exists(filePath))
            return true;
        return false;
    }

    private async Task<bool> DownloadFileWithRange(IApplicationVersion version, CancellationToken cancellationToken)
    {
        var uri = new Uri(version.DownloadUrl);
        var filePath = GetDownloadPath(version.DownloadUrl);

        long downloadedBytes = 0;
        var tempFile = filePath + ".temp";

        // file is already downloaded and renamed
        if (IsDownloaded(filePath))
            return true;

        var fileInfo = new FileInfo(tempFile);
        if (fileInfo.Exists)
        {
            if (await _hashUtil.VerifyFileMd5Async(tempFile, version.FileHash))
            {
                // File was downloaded already but not renamed
                _systemFile.Move(tempFile, filePath);
                return true;
            }

            downloadedBytes = fileInfo.Length;
        }

        var contentLength = await GetContentLength(uri, cancellationToken);

        if (contentLength <= downloadedBytes) // file is downloaded, but hashes don't match
            _systemFile.Delete(tempFile);

        if (contentLength > 0 && contentLength > downloadedBytes)
            downloadedBytes = await DownloadFileWithProgress(uri, contentLength, tempFile, cancellationToken, downloadedBytes);

        if (contentLength != downloadedBytes)
            return false;

        if (!await _hashUtil.VerifyFileMd5Async(tempFile, version.FileHash))
            return false;

        _systemFile.Move(tempFile, filePath);

        return true;

    }

    private async Task<long> DownloadFileWithProgress(Uri uri, long contentLength, string tempFile, CancellationToken cancellationToken, long downloadedBytes)
    {
        try
        {
            var request = new HttpRequestMessage { RequestUri = uri };

            if (downloadedBytes > 0)
                request.Headers.Range = new RangeHeaderValue(downloadedBytes, contentLength);

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var localFileStream = _systemFile.Open(tempFile, FileMode.Append, FileAccess.Write);

            var buffer = new byte[4096 * 4];
            int bytesRead;

            while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                downloadedBytes += bytesRead;
                var progressInPercent = (int)(downloadedBytes * 100 / contentLength);
                OnProgressChanged?.Invoke(this, new UpdateProgressChangedEventArgs(false, progressInPercent, downloadedBytes, contentLength));
                localFileStream.Write(buffer, 0, bytesRead);
            }

            _logger.Debug("Got bytes: {0}", downloadedBytes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error while downloading; Got bytes: {downloadedBytes} with Error:{ex.Message}");
        }

        return downloadedBytes;
    }

    private async Task<long> GetContentLength(Uri uri, CancellationToken cancellationToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Head, uri);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            if (response.Content.Headers.ContentLength.HasValue)
                return response.Content.Headers.ContentLength.Value;

            return 0;
        }
        catch (Exception ex)
        {
            _logger.Warn(ex, "Error while getting content length");
            return 0;
        }
    }


    public void AbortDownload()
    {
        if (_cancellationSource.Token.CanBeCanceled)
        {
            _httpClient?.CancelPendingRequests();
            _cancellationSource.Cancel();
        }
    }
}

public interface IUpdateDownloader
{
    Task StartDownloadAsync(IApplicationVersion version);

    event EventHandler<UpdateProgressChangedEventArgs> OnDownloadFinished;

    void AbortDownload();

    event EventHandler<UpdateProgressChangedEventArgs> OnProgressChanged;

    bool IsDownloaded(string filePath);

    string GetDownloadPath(string downloadUrl);

    DownloadSpeed DownloadSpeed { get; set; }
}
