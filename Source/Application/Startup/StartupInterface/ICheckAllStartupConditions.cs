using System;

namespace pdfforge.PDFCreator.Core.StartupInterface;

public interface ICheckAllStartupConditions
{
    /// <summary>
    /// Check all applicable startup conditions. If a condition fails, a StartupConditionFailedException will be thrown
    /// </summary>
    void CheckAll();
}

public class StartupConditionFailedException : Exception
{
    public int ExitCode { get; }

    public StartupConditionFailedException(int exitCode, string message) : base(message)
    {
        ExitCode = exitCode;
    }
}
