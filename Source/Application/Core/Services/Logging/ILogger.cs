using NLog;

namespace pdfforge.PDFCreator.Core.Services.Logging;

internal interface ILogger
{
    void ChangeLogLevel(LogLevel logLevel);

    string GetLogPath();
}
