
using System;
using Microsoft.Win32;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.HotFolder.Enums;

namespace pdfforge.PDFCreator.Core.SettingsManagement.Helper;

public interface IRegistryWrapper
{
    string GetValue(string keyPath, string valueName);
    void DeleteSubKeyTree(string keyPath);
}

public static class HotFolderMigrationHelper
{
    private const string OldHotFolderConfigPath = @"Software\pdfforge\HotFolder\Settings\HotFolderConfigs";
    private const string OldHotFolderGeneralSettingsPath = @"Software\pdfforge\HotFolder\Settings\GeneralSettings";
    private static IRegistryWrapper _registry = new DefaultRegistryWrapper();

    private class DefaultRegistryWrapper : IRegistryWrapper
    {
        public string GetValue(string keyPath, string valueName)
        {
            using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
            using var key = baseKey?.OpenSubKey(keyPath);
            return key?.GetValue(valueName, string.Empty)?.ToString();
        }
        public void DeleteSubKeyTree(string keyPath)
        {
            using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
            baseKey?.DeleteSubKeyTree(keyPath, false); // false = don't throw if key doesn't exist
        }
    }

    public static void SetRegistryWrapper(IRegistryWrapper registry)
        => _registry = registry ?? new DefaultRegistryWrapper();

    public static bool OldHotFolderExists()
    {
        try
        {
            return _registry.GetValue(OldHotFolderGeneralSettingsPath, "StartWithWindows") != null; // checking if one of the settings exists. It only gets created if HotFolder was launched at least once.
        }
        catch
        {
            return false;
        }
    }

    public static string GetOldHotFolderConfigCount()
    {
        try
        {
            var numClasses = _registry.GetValue(OldHotFolderConfigPath, "numClasses");
            return numClasses;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static string GetOldHotFolderAppStartPolicy()
    {
        try
        {
            return _registry.GetValue(OldHotFolderGeneralSettingsPath, "StartWhenApplicationOpens");
        }

        catch
        {
            return string.Empty;
        }
    }

    public static string GetOldHotFolderStartWithWindowsPolicy()
    {
        try
        {
            return _registry.GetValue(OldHotFolderGeneralSettingsPath, "StartWithWindows");
        }
        catch
        {
            return string.Empty;
        }
    }

    public static HotFolderConfig GetOldHotFolderConfig(int index)
    {
        var hotFolderConfig = new HotFolderConfig();
        var keyPath = $@"{OldHotFolderConfigPath}\{index}";

        hotFolderConfig.Printer = _registry.GetValue(keyPath, "Printer");
        hotFolderConfig.Guid = _registry.GetValue(keyPath, "Name");
        hotFolderConfig.FilterOption = Enum.Parse<FilterOption>(_registry.GetValue(keyPath, "FilterOption"));
        hotFolderConfig.HotFolderPath = _registry.GetValue(keyPath, "HotFolderPath");
        var oldSourceFileMover = MapOldDoNothingFileMover(_registry.GetValue(keyPath, "SourceFileMover"));
        hotFolderConfig.SourceFileMover = Enum.Parse<FileMover>(oldSourceFileMover);
        hotFolderConfig.SourceFilesPath = _registry.GetValue(keyPath, "SourceFilesPath");
        var oldUnprintableFileMover = MapOldDoNothingFileMover(_registry.GetValue(keyPath, "UnprintableFileMover"));
        hotFolderConfig.UnprintableFileMover = Enum.Parse<FileMover>(oldUnprintableFileMover);
        hotFolderConfig.UnprintableFilesPath = _registry.GetValue(keyPath, "UnprintableFilesPath");

        return hotFolderConfig;
    }

    private static string MapOldDoNothingFileMover(string oldFileMover)
    {
        if (oldFileMover.Equals("donothing", StringComparison.InvariantCultureIgnoreCase))
            return "SubFolder";
        return oldFileMover;
    }

    public static bool ConfigExists(int index)
    {
        try
        {
            return !string.IsNullOrEmpty(_registry.GetValue($@"{OldHotFolderConfigPath}\{index}", "Printer"));
        }
        catch
        {
            return false;
        }
    }
    public static int GetOldConfigFilterCount(int index)
    {
        try
        {
            var filterCount = _registry.GetValue($@"{OldHotFolderConfigPath}\{index}\Filter", "numClasses");
            return int.TryParse(filterCount, out var filterCountInt) ? filterCountInt : 0;
        }
        catch
        {
            return 0;
        }
    }

    public static string GetOldHotFolderFilter(int configIndex, int filterIndex)
    {
        try
        {
            return _registry.GetValue($@"{OldHotFolderConfigPath}\{configIndex}\Filter\{filterIndex}", "Filter");
        }
        catch
        {
            return string.Empty;
        }
    }

    public static void RemoveOldHotFolderIfNeeded()
    {
        if (OldHotFolderExists())
            RemoveOldHotFolderRegistry();
    }

    private static void RemoveOldHotFolderRegistry()
    {
        try
        {
            _registry.DeleteSubKeyTree(@"Software\pdfforge\HotFolder");
        }
        catch
        {
            // ignored
        }
    }
}
