using System.Diagnostics;
using Microsoft.Win32;
using SystemInterface.Microsoft.Win32;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;
public interface IAutoStartHelper
{
    void Register();
    void UnRegister();
    bool IsActive();
}

public class AutostartHelper : IAutoStartHelper
{
    private readonly IRegistry _registry;

    public AutostartHelper(IRegistry registry)
    {
        _registry = registry;
    }

    public void Register()
    {
        if (!IsActive())
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key?.SetValue("PDFCreator", $"\"{Process.GetCurrentProcess().MainModule?.FileName}\" /standby");
        }
    }

    public void UnRegister()
    {
        if (IsActive())
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key?.DeleteValue("PDFCreator");
        }
    }

    public bool IsActive()
    {
        using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false))
        {
            if (key?.GetValue("PDFCreator") != null)
                return true;
        }
        return false;
    }
}
