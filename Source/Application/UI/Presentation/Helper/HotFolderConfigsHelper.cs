using System.Linq;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.HotFolder.Enums;
using pdfforge.PDFCreator.Core.Printing.Printer;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.Helper;

public interface IHotFolderConfigsHelper
{
    void CheckHotFolderConfigs(PdfCreatorSettings settings);
}
public class HotFolderConfigsHelper : IHotFolderConfigsHelper
{
    private readonly IPrinterHelper _printerHelper;
    private readonly IGuid _guid;

    public HotFolderConfigsHelper(IPrinterHelper printerHelper, IGuid guid)
    {
        _printerHelper = printerHelper;
        _guid = guid;
    }

    public void CheckHotFolderConfigs(PdfCreatorSettings settings)
    {
        var printers = _printerHelper.GetPDFCreatorPrinters();

        foreach (var hotFolderConfig in settings.HotFolderSettings.HotFolderConfigs.ToList())
        {
            if (printers.All(p => p != hotFolderConfig.Printer))
                settings.HotFolderSettings.HotFolderConfigs.Remove(hotFolderConfig);
        }

        var hotFolderPrinterMappings = settings.ApplicationSettings.PrinterMappings
            .Where(pm => pm.IsHotFolder)
            .ToList();

        foreach (var mapping in hotFolderPrinterMappings)
        {
            if (settings.HotFolderSettings.HotFolderConfigs.All(hfc => hfc.Printer != mapping.PrinterName))
            {
                var newHotFolderConfig = new HotFolderConfig
                {
                    Guid = _guid.NewGuidString(),
                    Printer = mapping.PrinterName,
                    HotFolderPath = string.Empty,
                    IsActive = false,
                    FilterOption = FilterOption.NoFilter,
                    SourceFileMover = FileMover.Subfolder,
                    UnprintableFileMover = FileMover.Subfolder
                };

                settings.HotFolderSettings.HotFolderConfigs.Add(newHotFolderConfig);
            }
        }

    }
}

public class FreeHotFolderConfigsHelper : IHotFolderConfigsHelper
{
    public void CheckHotFolderConfigs(PdfCreatorSettings settings)
    {
        // Free edition does not support HotFolder, do nothing.
    }
}
