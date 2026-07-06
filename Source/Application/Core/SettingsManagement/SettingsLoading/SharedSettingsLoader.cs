using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.Utilities;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.SettingsManagement.SettingsLoading;

public interface ISharedSettingsLoader
{
    void ApplySharedSettings(PdfCreatorSettings currentSettings);

    IEnumerable<PrinterMapping> GetSharedPrinterMappings();

    string GetSharedSettingsIniFile();
}

public class SharedSettingsLoader : ISharedSettingsLoader
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private readonly IIniSettingsLoader _iniSettingsLoader;
    private readonly IDirectory _directory;
    private readonly IGpoSettings _gpoSettings;
    private readonly IProgramDataDirectoryHelper _programDataDirectoryHelper;

    public SharedSettingsLoader(IIniSettingsLoader iniSettingsLoader, IDirectory directory,
        IGpoSettings gpoSettings, IProgramDataDirectoryHelper programDataDirectoryHelper)
    {
        _iniSettingsLoader = iniSettingsLoader;
        _directory = directory;
        _gpoSettings = gpoSettings;
        _programDataDirectoryHelper = programDataDirectoryHelper;
    }

    public void ApplySharedSettings(PdfCreatorSettings currentSettings)
    {
        if (!_gpoSettings.LoadSharedAppSettings && !_gpoSettings.LoadSharedProfiles && !_gpoSettings.LoadSharedHotFolders)
            return;

        _logger.Info("Apply shared settings.");
        var sharedSettings = GetSharedSettings();
        if (sharedSettings == null)
            return;

        ApplyHotFolderSettings(currentSettings, sharedSettings);
        ApplyAppSettingsAndPrinterMappings(currentSettings, sharedSettings);
        ApplyProfiles(currentSettings, sharedSettings);
    }

    public string GetSharedSettingsIniFile()
    {
        try
        {
            var dir = _programDataDirectoryHelper.GetDir();
            var files = _directory.GetFiles(dir, "*.ini");
            foreach (var file in files)
            {
                if (string.Equals(PathSafe.GetFileNameWithoutExtension(file), _gpoSettings.SharedSettingsFilename, StringComparison.InvariantCultureIgnoreCase))
                    return file;
            }
            if (files.Length > 0)
                return files[0];
        }
        catch { }
        return null;
    }

    private void ApplyAppSettingsAndPrinterMappings(PdfCreatorSettings currentSettings, PdfCreatorSettings sharedSettings)
    {
        //For a transitional period, both GPOs will remain to control printer mappings. Ticket to remove afterward: PC-5664
        var loadPrinterMappings = (_gpoSettings.LoadSharedPrinterMappings || _gpoSettings.DisablePrinterTab);

        if (_gpoSettings.LoadSharedAppSettings || loadPrinterMappings)
        {
            _logger.Info("Apply shared app settings.");

            if (_gpoSettings.LoadSharedAppSettings)
            {
                //Preserve current printer mappings before they get overwritten
                var currentPrinterMapping = new PrinterMapping[currentSettings.ApplicationSettings.PrinterMappings.Count];
                currentSettings.ApplicationSettings.PrinterMappings.CopyTo(currentPrinterMapping, 0);

                //Load all shared app settings
                currentSettings.ApplicationSettings = sharedSettings.ApplicationSettings;

                //Restore current printer mappings to be able to load the printer mappings separately
                if (!loadPrinterMappings)
                    currentSettings.ApplicationSettings.PrinterMappings = new ObservableCollection<PrinterMapping>(currentPrinterMapping);

                currentSettings.CreatorAppSettings = sharedSettings.CreatorAppSettings;
            }

            if (loadPrinterMappings)
            {
                currentSettings.ApplicationSettings.PrinterMappings = sharedSettings.ApplicationSettings.PrinterMappings;
            }
        }
    }

    private void ApplyProfiles(PdfCreatorSettings currentSettings, PdfCreatorSettings sharedSettings)
    {
        if (!_gpoSettings.LoadSharedProfiles)
            return;

        _logger.Info("Apply shared profiles.");
        if (_gpoSettings.AllowUserDefinedProfiles)
        {
            var additionalProfiles = new List<ConversionProfile>();

            foreach (var currentProfile in currentSettings.ConversionProfiles)
            {
                //do not add current profiles which were previously shared
                if (!currentProfile.Properties.IsShared && !ProfileExists(currentProfile, sharedSettings.ConversionProfiles))
                    additionalProfiles.Add(currentProfile);
            }
            foreach (var profile in additionalProfiles)
                sharedSettings.ConversionProfiles.Add(profile);
        }

        currentSettings.ConversionProfiles = sharedSettings.ConversionProfiles;
    }

    private bool ProfileExists(ConversionProfile profile, IList<ConversionProfile> profiles)
    {
        return profiles.Any(p => p.Name == profile.Name || p.Guid == profile.Guid);
    }

    public IEnumerable<PrinterMapping> GetSharedPrinterMappings()
    {
        if ((_gpoSettings.LoadSharedAppSettings && _gpoSettings.DisablePrinterTab) || _gpoSettings.LoadSharedPrinterMappings)
        {
            var sharedSettings = GetSharedSettings();
            if (sharedSettings != null)
                return sharedSettings.ApplicationSettings.PrinterMappings;
        }

        return new List<PrinterMapping>();
    }

    private PdfCreatorSettings GetSharedSettings()
    {
        var iniFile = GetSharedSettingsIniFile();
        if (iniFile == null)
        {
            _logger.Debug("Could not find shared settings.ini.");
            return null;
        }

        try
        {
            _logger.Info("Get shared settings from '" + iniFile + "'.");

            var sharedSettings = (PdfCreatorSettings)_iniSettingsLoader.LoadIniSettings(iniFile);
            foreach (var profile in sharedSettings.ConversionProfiles)
                profile.Properties.IsShared = true;

            return sharedSettings;
        }
        catch
        {
            _logger.Warn("Could not load settings from '" + iniFile + "'.");
            return null;
        }
    }

    private void ApplyHotFolderSettings(PdfCreatorSettings currentSettings, PdfCreatorSettings sharedSettings)
    {
        if (!_gpoSettings.LoadSharedHotFolders)
            return;

        _logger.Info("Apply HotFolder settings");
        currentSettings.HotFolderSettings = sharedSettings.HotFolderSettings;
    }
}

public class FreeSharedSettingsLoader : ISharedSettingsLoader
{
    public void ApplySharedSettings(PdfCreatorSettings currentSettings)
    {
    }

    public IEnumerable<PrinterMapping> GetSharedPrinterMappings()
    {
        return new List<PrinterMapping>();
    }

    public string GetSharedSettingsIniFile()
    {
        return null;
    }
}
