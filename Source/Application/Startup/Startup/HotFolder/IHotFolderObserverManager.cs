using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Core.Startup.HotFolder;

public interface IHotFolderObserverManager
{
    void StartWatchingAll();

    void StopWatchingAll();

    void StartWatching(HotFolderConfig config, bool sendStatistics = false);
    void StopWatching(HotFolderConfig config);
    bool IsRunning(HotFolderConfig hotFolderConfig);
}
