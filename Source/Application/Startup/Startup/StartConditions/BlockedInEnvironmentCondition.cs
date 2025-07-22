using NLog;
using pdfforge.PDFCreator.Core.Services.EnvironmentDetection;
using pdfforge.PDFCreator.Core.Services.Licensing;
using pdfforge.PDFCreator.Core.StartupInterface;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Messages.ErrorMessages;

namespace pdfforge.PDFCreator.Core.Startup.StartConditions;
public class BlockedInEnvironmentCondition(ITerminalServerDetection terminalServerDetection, IDomainDetector domainDetector, IExitMessageHelper exitMessageHelper, EditionHelper editionHelper)
    : IStartupCondition
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    public bool CanRequestUserInteraction => true;

    // This condition is only registered in Professional and Free bootstrappers, so terminal server edition (we allow it everywhere) will ignore this.
    public StartupConditionResult Check()
    {
#if !DEVELOPMENT_MODE // this is needed for us to develop Free without having it blocked, because we're in a domain.

        if (terminalServerDetection.IsWindowsEnterpriseMultiSession())
        {
            if (!editionHelper.IsProfessional)
            {
                return HandleExitState(ExitCode.BlockedInEnterpriseMultiSession);
            }
        }
        else if (terminalServerDetection.IsTerminalServer())
        {
            if (!editionHelper.IsTerminalServer)
            {
                return HandleExitState(ExitCode.NotValidOnTerminalServer);
            }
        }
        else if (domainDetector.ComputerIsPartOfDomain())
        {
            if (!editionHelper.IsProfessional)
            {
                return HandleExitState(ExitCode.BlockedInDomain);
            }
        }
#endif
        return StartupConditionResult.BuildSuccess();
    }
    private StartupConditionResult HandleExitState(ExitCode code)
    {
        exitMessageHelper.ShowMessage((int)code);
        return StartupConditionResult.BuildErrorWithMessage((int)code, "", showMessage: false);
    }
}
