using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace pdfforge.PDFCreator.Core.Services;

public interface ILanguageLoader
{
    string GetTranslationFile(string language);

    string GetTranslationFileIfExists(string language);

    IEnumerable<Language> GetAvailableLanguages();

    Language FindBestLanguage(CultureInfo cultureInfo);

    Language FindBestLanguage(CultureInfo cultureInfo, IEnumerable<Language> languages);

    string TranslationFolder { get; }
}

public class GettextLanguageLoader : ILanguageLoader
{
    private readonly Language _englishLanguage = new Language
    {
        CommonName = "English",
        NativeName = "English",
        Iso2 = "en",
        CultureInfo = new CultureInfo("en")
    };

    private readonly Dictionary<string, Language> _predefinedLanguages;

    public GettextLanguageLoader(IEnumerable<string> translationFolderCandidates)
    {
        TranslationFolder = FindTranslationFolder(translationFolderCandidates);

        _predefinedLanguages = new Dictionary<string, Language>
        {
            { "en", _englishLanguage},
            {"val", new Language() {Iso2 = "val", CommonName = "Valencian", NativeName = "valencià"} }
        };
    }

    private string FindTranslationFolder(IEnumerable<string> translationFolderCandidates)
    {
        foreach (var directory in translationFolderCandidates)
        {
            if (!Directory.Exists(directory))
                continue;

            if (Directory.EnumerateFiles(directory, "*.mo", SearchOption.AllDirectories).Any())
            {
                return directory;
            }
        }

        return "";
    }

    public string GetTranslationFile(string language)
    {
        return GetTranslationFileIfExists(language) ?? "en";
    }

    public string GetTranslationFileIfExists(string language)
    {
        var languages = GetAvailableLanguages();
        return languages.FirstOrDefault(x => x.Iso2 == language)?.Iso2;
    }

    private Language FindLanguageByCulture(string name)
    {
        var cultureInfo = CultureInfo.GetCultureInfo(name);
        var language = new Language();

        if (cultureInfo.EnglishName.StartsWith("Unknown Language", StringComparison.InvariantCultureIgnoreCase))
            throw new CultureNotFoundException("The culture could not be found", name);

        language.CultureInfo = cultureInfo;
        language.CommonName = cultureInfo.EnglishName;
        language.NativeName = cultureInfo.NativeName;
        language.Iso2 = cultureInfo.Name;

        return language;
    }

    private Language FindLanguage(string name)
    {
        try
        {
            return FindLanguageByCulture(name);
        }
        catch (CultureNotFoundException)
        {
        }

        return _predefinedLanguages.ContainsKey(name)
            ? _predefinedLanguages[name]
            : null;
    }

    public IEnumerable<Language> GetAvailableLanguages()
    {
        var languages = new List<Language>();
        languages.Add(_englishLanguage);

        if (!Directory.Exists(TranslationFolder))
            return languages;

        foreach (var directory in Directory.EnumerateDirectories(TranslationFolder))
        {
            if (!Directory.EnumerateFiles(directory, "*.mo", SearchOption.AllDirectories).Any())
                continue;

            var cultureName = Path.GetFileName(directory);
            var lang = FindLanguage(cultureName);
            if (lang != null)
                languages.Add(lang);
        }

        return languages.OrderBy(x => x.CommonName);
    }

    public Language FindBestLanguage(CultureInfo cultureInfo)
    {
        if (cultureInfo.EnglishName.StartsWith("Unknown Language", StringComparison.InvariantCultureIgnoreCase))
            throw new CultureNotFoundException("The culture is not known", cultureInfo.Name);

        var languagesWithCultureInfo = GetAvailableLanguages().Where(x => x.CultureInfo != null).ToList();

        var language = languagesWithCultureInfo.FirstOrDefault(x => x.CultureInfo.Equals(cultureInfo));

        if (language != null)
            return language;

        language = languagesWithCultureInfo.FirstOrDefault(x => x.CultureInfo.TwoLetterISOLanguageName == cultureInfo.TwoLetterISOLanguageName);

        if (language != null)
            return language;

        language = languagesWithCultureInfo.FirstOrDefault(x => x.CultureInfo.Equals(CultureInfo.CurrentUICulture));

        if (language != null)
            return language;

        return _englishLanguage;
    }

    public Language FindBestLanguage(CultureInfo cultureInfo, IEnumerable<Language> languages)
    {
        throw new NotImplementedException();
    }

    public string TranslationFolder { get; }
}
