using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Core.Startup.HotFolder;

public interface IHotFolderManager
{
    void StartAll();
    void Stop(HotFolderConfig config);
    void Start(HotFolderConfig config);
    void UpdateSetting(HotFolderConfig config);
    void StopAll();
}
