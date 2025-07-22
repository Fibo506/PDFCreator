using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace pdfforge.PDFCreator.UI.Presentation.Workflow;

public interface IWpfTopMostHelper
{
    void ShowTopMost(Window window, bool revertWhenActive);

    bool? ShowDialogTopMost(Window window, bool revertWhenActive);

    void MakeTopMostWindow(Window window, bool revertWhenActive);
}

public class WpfTopMostHelper : IWpfTopMostHelper
{
    public void MakeTopMostWindow(Window window, bool revertWhenActive)
    {
        window.Topmost = true;
        window.Activate();

        if (revertWhenActive)
            window.Activated += WindowActivated;

        window.Loaded += WindowLoad;
    }

    private void UndoTopMostWindow(Window window)
    {
        window.Topmost = false;
    }

    public void ShowTopMost(Window window, bool revertWhenActive)
    {
        MakeTopMostWindow(window, revertWhenActive);
        window.Show();
    }

    public bool? ShowDialogTopMost(Window window, bool revertWhenActive)
    {
        MakeTopMostWindow(window, revertWhenActive);
        return window.ShowDialog();
    }

    private void WindowActivated(object sender, EventArgs eventArgs)
    {
        var window = sender as Window;

        if (window != null)
        {
            UndoTopMostWindow(window);
            window.Activated -= WindowActivated;
        }
    }

    private void WindowLoad(object sender, EventArgs eventArgs)
    {
        var window = sender as Window;

        if (window != null)
        {
            var windowHandle = GetWindowHandle(window);
            SetForegroundWindow(windowHandle);
        }
    }

    private IntPtr GetWindowHandle(Window window)
    {
        return new WindowInteropHelper(window).Handle;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
}
