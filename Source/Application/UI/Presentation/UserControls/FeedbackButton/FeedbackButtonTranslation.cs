using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls;

internal class FeedbackButtonTranslation : ITranslatable
{
    public string SendFeedback { get; private set; } = "Send us feedback";
    public string SendYourFeedback { get; private set; } = "Send us your feedback";

    public string PrioritySupport { get; private set; } = "Priority Support";
}
