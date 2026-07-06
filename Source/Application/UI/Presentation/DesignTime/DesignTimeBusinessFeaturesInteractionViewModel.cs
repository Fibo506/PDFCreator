using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.Windows.ProfessionalFeatureInteractions;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeBusinessFeaturesInteractionViewModel : BusinessFeaturesInteractionViewModel
{
    public DesignTimeBusinessFeaturesInteractionViewModel() : base(new DesignTimeTranslationUpdater(), new DesignTimeCommandLocator())
    {
    }
}
