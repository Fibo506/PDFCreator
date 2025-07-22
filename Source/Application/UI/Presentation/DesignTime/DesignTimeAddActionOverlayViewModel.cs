using System.Collections.Generic;
using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.Helper.ActionHelper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeAddActionOverlayViewModel : AddActionOverlayViewModel
{
    public DesignTimeAddActionOverlayViewModel() :
        base(new DesignTimeEventAggregator(), new DesignTimeCurrentSettingsProvider(),
            new List<IPresenterActionFacade>(), new DesignTimeTranslationUpdater(), new DesignTimeCommandLocator(),
            new DesignTimeEditionHelper())
    {
    }
}
