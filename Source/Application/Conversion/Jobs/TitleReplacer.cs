using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.Conversion.Jobs;

/// <summary>
///     Replaces occurances within a string in a given order. This is used to remove unwanted parts from titles in the
///     JobInfos
/// </summary>
public class TitleReplacer
{

    private readonly List<TitleReplacement> _replacements = new List<TitleReplacement>();
    private readonly List<List<char>> _variantGroups = new List<List<char>>();
    private readonly List<TitleReplacement> _variantReplacement = new List<TitleReplacement>();

    public TitleReplacer()
    {
        AddVariant([(char)0x2E3B, (char)0x2E3A, (char)0x2013, (char)0x2014, (char)0x2015, '-']);
    }
    /// <summary>
    ///     Replace the title string with the replacements
    /// </summary>
    /// <param name="originalTitle">The original title where replacements should be applied</param>
    /// <returns>The title with replacements</returns>
    public string Replace(string originalTitle)
    {
        if (originalTitle == null)
            throw new ArgumentException("originalTitle");

        var title = originalTitle;

        var replacements = new List<TitleReplacement>();
        replacements.AddRange(_replacements);
        replacements.AddRange(_variantReplacement);

        // Descending to replace longer strings first to avoid e.g. replacement of .doc before .docx
        var sortedReplacements = replacements
            .OrderBy(x => x.ReplacementType)
            .ThenByDescending(replacement => replacement.Search.Length)
            .ThenByDescending(y => y.Search);

        foreach (var titleReplacement in sortedReplacements)
        {
            if (titleReplacement.IsValid())
                title = ReplaceTitle(titleReplacement, title);
        }

        return title;
    }

    private string ReplaceTitle(TitleReplacement titleReplacement, string title)
    {
        if (string.IsNullOrEmpty(titleReplacement.Search))
            return title;

        switch (titleReplacement.ReplacementType)
        {
            case ReplacementType.RegEx:
                title = Regex.Replace(title, titleReplacement.Search, titleReplacement.Replace);
                break;

            case ReplacementType.Start:
                if (title.StartsWith(titleReplacement.Search, StringComparison.InvariantCultureIgnoreCase))
                {
                    title = title.Substring(titleReplacement.Search.Length);
                }
                break;

            case ReplacementType.End:
                if (title.EndsWith(titleReplacement.Search, StringComparison.InvariantCultureIgnoreCase))
                {
                    title = title.Substring(0, title.LastIndexOf(titleReplacement.Search, StringComparison.InvariantCultureIgnoreCase));
                }
                break;

            case ReplacementType.Replace:
            default:
                title = title.Replace(titleReplacement.Search, "", StringComparison.InvariantCultureIgnoreCase);
                break;
        }

        return title;
    }

    public void AddReplacement(TitleReplacement titleReplacement)
    {
        _replacements.Add(titleReplacement);

        // don't replace based on variant when using explicit replacements
        if (titleReplacement.ReplacementType is ReplacementType.RegEx or ReplacementType.Replace)
            return;

        foreach (var variantGroup in _variantGroups)
        {
            foreach (var c in variantGroup)
            {
                if (titleReplacement.Search.Contains(c))
                {
                    var list = CreateVariantTitleReplacementList(titleReplacement, variantGroup, c);
                    foreach (var variant in list)
                    {
                        _variantReplacement.Add(new TitleReplacement(titleReplacement.ReplacementType, variant, titleReplacement.Replace));
                    }
                    break;
                }
            }
        }
    }

    private List<string> CreateVariantTitleReplacementList(TitleReplacement titleReplacement, List<char> variantGroup, char baseChar)
    {
        var list = new List<string>();
        foreach (var variantChar in variantGroup)
        {
            if (variantChar == baseChar)
                continue;
            var variantSearch = titleReplacement.Search.Replace(baseChar, variantChar);
            list.Add(variantSearch);
        }

        return list;
    }

    public void AddReplacements(IEnumerable<TitleReplacement> replacements)
    {
        foreach (var titleReplacement in replacements)
        {
            AddReplacement(titleReplacement);
        }
    }

    public void AddVariant(List<char> variantGroup)
    {
        _variantGroups.Add(variantGroup);
    }
}

internal static class StringExtension
{
    public static string Replace(this string source, string oldString, string newString, StringComparison comparison)
    {
        int index = source.IndexOf(oldString, comparison);

        while (index > -1)
        {
            source = source.Remove(index, oldString.Length);
            source = source.Insert(index, newString);

            index = source.IndexOf(oldString, index + newString.Length, comparison);
        }

        return source;
    }
}
