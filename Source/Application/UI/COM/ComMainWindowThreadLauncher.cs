using pdfforge.PDFCreator.Core.Controller;

namespace pdfforge.PDFCreator.UI.COM;

internal class ComMainWindowThreadLauncher : IMainWindowThreadLauncher
{
    public void LaunchMainWindow()
    {
    }

    public bool IsPrintJobShellOpen()
    {
        return true;
    }

    public void SwitchPrintJobShellToMergeWindow()
    {
    }
}
