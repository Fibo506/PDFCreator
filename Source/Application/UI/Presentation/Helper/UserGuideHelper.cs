using System;
using System.Collections.Generic;
using System.Linq;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;
using pdfforge.PDFCreator.UI.Presentation.Help;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.UserGuide;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public class UserGuideHelper : IUserGuideHelper
{
    private readonly IAssemblyHelper _assemblyHelper;
    private readonly IDirectory _directoryWrapper;
    private readonly IPath _pathWrapper;
    private readonly IUserGuideLauncher _userGuideLauncher;
    private readonly IApplicationLanguageProvider _applicationLanguageProvider;
    private readonly ILanguageProvider _languageProvider;

    public UserGuideHelper(IDirectory directoryWrapper, IPath pathWrapper, IAssemblyHelper assemblyHelper, IUserGuideLauncher userGuideLauncher, IApplicationLanguageProvider applicationLanguageProvider, ILanguageProvider languageProvider)
    {
        _directoryWrapper = directoryWrapper;
        _pathWrapper = pathWrapper;
        _assemblyHelper = assemblyHelper;
        _userGuideLauncher = userGuideLauncher;
        _applicationLanguageProvider = applicationLanguageProvider;
        _languageProvider = languageProvider;

        UpdateLanguage();

        _applicationLanguageProvider.LanguageChanged += OnLanguageChanged;
    }

    private Language GetLanguage()
    {
        var englishLanguage = _languageProvider.FindBestLanguage("en");
        var languageIso = _applicationLanguageProvider.GetApplicationLanguage();
        var language = _languageProvider.GetAvailableLanguages().FirstOrDefault(lang => lang.Iso2 == languageIso);

        return language ?? englishLanguage;
    }

    public void ShowHelp(HelpTopic topic)
    {
        _userGuideLauncher.ShowHelpTopic(topic);
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        var language = GetLanguage();

        // used for debugging UserGuide by setting a custom location
        var envVar = Environment.GetEnvironmentVariable("PDFCreatorUserGuideWebsite");
        var applicationDir = _assemblyHelper.GetAssemblyDirectory();

        var candidates = new[]
        {
            $"{applicationDir}\\UserGuide",
            "..\\..\\..\\..\\..\\..\\..\\..\\packages\\test\\PDFCreator.UserGuide\\content\\inapp",
            "C:\\Program Files\\PDFCreator\\UserGuide",
            "C:\\Program Files\\PDFCreator Server\\UserGuide"
        }
            .Select(_pathWrapper.GetFullPath)
            .ToList();

        if (!string.IsNullOrWhiteSpace(envVar))
            candidates.Insert(1, envVar);

        foreach (var candidate in candidates)
        {
            if (_directoryWrapper.Exists(PathSafe.Combine(candidate, language.Iso2)))
            {
                _userGuideLauncher.SetUserGuide(candidate, language.Iso2);
                return;
            }
        }

        _userGuideLauncher.SetLanguage("en");
    }
}
