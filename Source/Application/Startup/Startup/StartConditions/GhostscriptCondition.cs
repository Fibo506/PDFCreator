using pdfforge.PDFCreator.Conversion.Ghostscript;
using pdfforge.PDFCreator.Core.Startup.Translations;
using pdfforge.PDFCreator.Core.StartupInterface;
using Translatable;

namespace pdfforge.PDFCreator.Core.Startup.StartConditions;

public class GhostscriptCondition : IStartupCondition
{
    private readonly IGhostscriptDiscovery _ghostscriptDiscovery;
    private readonly StartupTranslation _translation;

    public bool CanRequestUserInteraction => false;

    public GhostscriptCondition(IGhostscriptDiscovery ghostscriptDiscovery, ITranslationFactory translationFactory)
    {
        _ghostscriptDiscovery = ghostscriptDiscovery;
        _translation = translationFactory.CreateTranslation<StartupTranslation>();
    }

    public StartupConditionResult Check()
    {
        if (!GhoscriptIsInstalled())
            return StartupConditionResult.BuildErrorWithMessage((int)ExitCode.GhostScriptNotFound, _translation.NoSupportedGSFound);

        return StartupConditionResult.BuildSuccess();
    }

    private bool GhoscriptIsInstalled()
    {
        var gsVersion = _ghostscriptDiscovery.GetGhostscriptInstance();

        return gsVersion != null;
    }
}
