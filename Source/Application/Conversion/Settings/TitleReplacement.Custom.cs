using System;
using System.Text.RegularExpressions;
using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.Conversion.Settings;

public partial class TitleReplacement
{
    public TitleReplacement()
    {

    }

    /// <summary>
    ///     Create a TitleReplacement with search and replace fields set
    /// </summary>
    /// <param name="replacementType">Defines how the replacement will be processed</param>
    /// <param name="search">The text to search for</param>
    /// <param name="replace">The text that will be inserted instead</param>
    public TitleReplacement(ReplacementType replacementType, string search, string replace)
    {
        ReplacementType = replacementType;
        Search = search;
        Replace = replace;
    }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Search))
            return false;

        if (ReplacementType != ReplacementType.RegEx)
            return true;

        try
        {
            Regex.Replace("", Search, Replace, RegexOptions.IgnoreCase);
        }
        catch (ArgumentException)
        {
            return false;
        }

        return true;
    }
}
