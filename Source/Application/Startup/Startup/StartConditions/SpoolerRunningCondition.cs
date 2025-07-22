using System;
using System.ComponentModel;
using System.Diagnostics;
using NLog;
using pdfforge.PDFCreator.Core.Startup.Translations;
using pdfforge.PDFCreator.Core.StartupInterface;
using Translatable;

namespace pdfforge.PDFCreator.Core.Startup.StartConditions;

public class SpoolerRunningCondition : IStartupCondition
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly StartupTranslation _translation;

    public bool CanRequestUserInteraction => false;

    public SpoolerRunningCondition(ITranslationFactory translationFactory)
    {
        _translation = translationFactory.CreateTranslation<StartupTranslation>();
    }

    public StartupConditionResult Check()
    {
        if (SpoolerIsRunning())
            return StartupConditionResult.BuildSuccess();

        return StartupConditionResult.BuildErrorWithMessage((int)ExitCode.SpoolerNotRunning, _translation.SpoolerNotRunning);
    }

    private bool SpoolerIsRunning()
    {
        try
        {
            return Process.GetProcessesByName("spoolsv").Length > 0;
        }
        catch (InvalidOperationException ex)
        {
            var win32Exception = ex.InnerException as Win32Exception;
            if (win32Exception?.NativeErrorCode == 5)
            {
                _logger.Warn(ex, "Could not check spooler status: We do not have sufficient privileges.");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Could not check spooler status");
            return false;
        }
    }
}
