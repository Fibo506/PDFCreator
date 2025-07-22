using System.Windows;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public interface IClipboardService
{
    void SetDataObject(object data);
}

public class ClipboardService : IClipboardService
{
    public void SetDataObject(object data)
    {
        Clipboard.SetDataObject(data, true);
    }
}
