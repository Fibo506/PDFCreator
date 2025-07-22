using System;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public class PrismNavigationValueObject
{
    public string RegionName { get; }
    public string TargetView { get; }
    public Action Activate { get; }

    public PrismNavigationValueObject(string regionName, string targetView, Action activate)
    {
        this.RegionName = regionName;
        this.TargetView = targetView;
        Activate = activate;
    }
}
