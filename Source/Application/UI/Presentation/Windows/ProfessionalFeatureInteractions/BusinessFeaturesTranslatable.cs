using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.Windows.ProfessionalFeatureInteractions;

public class BusinessFeaturesTranslatable : ITranslatable
{
    public string LeftSideHeaderString { get; private set; } = "Ready to do more than just the basics?";
    public string LeftSideDescription { get; private set; } = "Unlock PDFCreator's premium features to create smarter workflows and save time.";
    public string RightSideHeader { get; private set; } = "Business features:";
    public string Row1Feature { get; private set; } = "256-bit encryption";
    public string Row2Feature { get; private set; } = "HotFolder";
    public string Row3Feature { get; private set; } = "User Tokens";
    public string Row4Feature { get; private set; } = "Delete pages";
    public string Row5Feature { get; private set; } = "Forward to profile";
    public string Row6Feature { get; private set; } = "Priority support";
    public string Row7Feature { get; private set; } = "Group Policies";
    public string Row8Feature { get; private set; } = "Shared settings";
    public string Row9Feature { get; private set; } = "MSI installer";
    public string UpgradeNowButton { get; private set; } = "Upgrade Now";

}
