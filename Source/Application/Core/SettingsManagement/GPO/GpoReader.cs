using Microsoft.Win32;
using NLog;
using pdfforge.DataStorage;
using pdfforge.DataStorage.Storage;
using pdfforge.PDFCreator.Core.SettingsManagement.GPO.Settings;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.SettingsManagement.GPO;

public class GpoReader<T> where T : IGeneratedGpoSettings
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly IStorage _hkcuStorage;
    private readonly IStorage _hklmStorage;
    private readonly RegistryStorage _hklmRestrictions;

    public GpoReader(string applicationRegKey)
    {
        var policyPath = PathSafe.Combine(@"Software\Policies\", applicationRegKey);
        _hkcuStorage = new RegistryStorage(RegistryHive.CurrentUser, policyPath);
        _hklmStorage = new RegistryStorage(RegistryHive.LocalMachine, policyPath);

        var customRestrictionsPath = @"Software\pdfforge\PDFCreator\Restrictions";
        _hklmRestrictions = new RegistryStorage(RegistryHive.LocalMachine, customRestrictionsPath);
    }

    internal GpoReader(IStorage hklmStorage, IStorage hkcuStorage)
    {
        _hkcuStorage = hkcuStorage;
        _hklmStorage = hklmStorage;
    }

    public T ReadGpoSettings(T settings)
    {
        var data = Data.CreateDataStorage();
        TryReadData(_hklmRestrictions, data);
        TryReadData(_hklmStorage, data);
        TryReadData(_hkcuStorage, data);

        settings.ReadValues(data);

        _logger.Info("GpoSettings applied.");

        return settings;
    }

    private void TryReadData(IStorage storage, Data data)
    {
        try
        {
            storage.ReadData(data);
        }
        catch
        {
            _logger.Debug("Policy path does not exist.");
        }
    }
}
