using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using NaturalSort.Extension;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Profiles;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Services;

public interface IPrinterMappingService
{
    ObservableCollection<PrinterMappingWrapper> GetPrinterMappings();
}

public class PrinterMappingService : IPrinterMappingService
{
    private readonly ICurrentSettings<ObservableCollection<PrinterMapping>> _printerMappingProvider;
    private readonly ICurrentSettings<ObservableCollection<ConversionProfile>> _profilesProvider;

    public ObservableCollection<PrinterMappingWrapper> PrinterMappings { get; set; }

    public PrinterMappingService(ICurrentSettings<ObservableCollection<PrinterMapping>> printerMappingProvider,
        ICurrentSettings<ObservableCollection<ConversionProfile>> profilesProvider)
    {
        _printerMappingProvider = printerMappingProvider;
        _profilesProvider = profilesProvider;
    }

    public ObservableCollection<PrinterMappingWrapper> GetPrinterMappings()
    {

        var printerMappings = SetupPrinterMappings();

        if (printerMappings == null)
            return [];

        PrinterMappings = printerMappings.ObservableCollection
            .OrderBy(x => x.PrinterName, StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToObservableCollection();
        PrinterMappings.CollectionChanged += PrinterMappings_OnCollectionChanged;

        return PrinterMappings;
    }


    private Helper.SynchronizedCollection<PrinterMappingWrapper> SetupPrinterMappings()
    {
        if (_printerMappingProvider?.Settings == null)
            return null;

        var mappingWrappers = new List<PrinterMappingWrapper>();

        foreach (var printerMapping in _printerMappingProvider.Settings)
        {
            var mappingWrapper = new PrinterMappingWrapper(printerMapping, GetConversionProfiles());
            mappingWrappers.Add(mappingWrapper);
        }

        return new Helper.SynchronizedCollection<PrinterMappingWrapper>(mappingWrappers);
    }

    private ObservableCollection<ConversionProfileWrapper> GetConversionProfiles()
    {
        return _profilesProvider.Settings
            .Select(x => new ConversionProfileWrapper(x))
            .OrderBy(pm => pm.Name, StringComparison.OrdinalIgnoreCase.WithNaturalSort())
            .ToObservableCollection();
    }

    private void PrinterMappings_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        _printerMappingProvider.Settings.Clear();

        foreach (var printerMappingWrapper in PrinterMappings)
        {
            _printerMappingProvider.Settings.Add(printerMappingWrapper.PrinterMapping);
        }
    }
}
