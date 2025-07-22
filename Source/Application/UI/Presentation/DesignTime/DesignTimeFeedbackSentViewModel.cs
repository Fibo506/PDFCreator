using pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Feedback;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

internal class DesignTimeFeedbackSentViewModel : FeedbackSentViewModel
{
    public DesignTimeFeedbackSentViewModel() : base(new DesignTimeTranslationUpdater())
    {
    }
}
