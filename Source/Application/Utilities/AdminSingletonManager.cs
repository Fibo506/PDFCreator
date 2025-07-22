using System;
using pdfforge.PDFCreator.Utilities.Threading;

namespace pdfforge.PDFCreator.Utilities;

public interface IAdminSingletonManager
{
    void Initialize(string adminMutexName);
    bool IsFirstAdminInstance();
    void Release();
}

public class AdminSingletonManager : IAdminSingletonManager
{
    private bool _isFirstAdminInstance;
    private LocalMutex _mutex;

    /// <summary>
    /// Initializes the admin singleton manager and determines if this is the first admin instance by trying to get the admin mutex.
    /// </summary>
    public void Initialize(string adminMutexName)
    {
        _mutex = new LocalMutex(adminMutexName);
        _isFirstAdminInstance = _mutex.Acquire(TimeSpan.FromSeconds(1));
    }
    public void Release()
    {
        _mutex?.Release();
    }

    public bool IsFirstAdminInstance() => _isFirstAdminInstance;
}
