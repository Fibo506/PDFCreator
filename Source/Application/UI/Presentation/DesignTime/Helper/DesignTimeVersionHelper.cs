using System;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

public class DesignTimeVersionHelper : IVersionHelper
{
    public Version ApplicationVersion => new Version(1, 2, 3, 4);

    public string FormatWithTwoDigits()
    {
        return "1.2";
    }

    public string FormatWithThreeDigits()
    {
        return "1.2.3";
    }

    public string FormatWithBuildNumber()
    {
        return "1.2.3 Build 4";
    }
}
