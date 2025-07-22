using System.Globalization;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.SettingsManagement;
using pdfforge.PDFCreator.Core.SettingsManagement.DefaultSettings;
using pdfforge.PDFCreator.Core.SettingsManagement.SettingsLoading;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;

namespace pdfforge.PDFCreator.UI.COM;

internal class ComSettingsLoader : SettingsLoader
{
    private readonly ITranslationHelper _translationHelper;

    public ComSettingsLoader(ISettingsMover settingsMover,
        IInstallationPathProvider installationPathProvider,
        IDefaultSettingsBuilder defaultSettingsBuilder,
        IMigrationStorageFactory migrationStorageFactory,
        IActionOrderHelper actionOrderHelper,
        ISettingsBackup settingsBackup,
        ITranslationHelper translationHelper,
        ISharedSettingsLoader sharedSettingsLoader,
        IBaseSettingsBuilder baseSettingsBuilder,
        IGpoSettings gpoSettings)
        : base(settingsMover, installationPathProvider, defaultSettingsBuilder, migrationStorageFactory, actionOrderHelper, settingsBackup, sharedSettingsLoader, baseSettingsBuilder, gpoSettings)
    {
        _translationHelper = translationHelper;
    }

    protected override void ProcessBeforeSaving(PdfCreatorSettings settings)
    { }

    protected override void ProcessAfterSaving(PdfCreatorSettings settings)
    { }

    protected override void PrepareForLoading()
    { }

    protected override void ProcessAfterLoading(PdfCreatorSettings settings)
    {
        _translationHelper.TranslateProfileList(settings.ConversionProfiles);
        CheckLanguage(settings);
        CheckUpdateInterval(settings);
    }

    private void CheckUpdateInterval(PdfCreatorSettings settings)
    {
    }

    private void CheckLanguage(PdfCreatorSettings settings)
    {
        if (!_translationHelper.HasTranslation(settings.ApplicationSettings.Language))
        {
            var language = _translationHelper.FindBestLanguage(CultureInfo.CurrentUICulture);

            var setupLanguage = _translationHelper.SetupLanguage;
            if (!string.IsNullOrWhiteSpace(setupLanguage) && _translationHelper.HasTranslation(setupLanguage))
                language = _translationHelper.FindBestLanguage(setupLanguage);

            settings.ApplicationSettings.Language = language.Iso2;
        }
    }
}
