using System;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Core.ComImplementation;
using pdfforge.PDFCreator.Core.Services.Logging;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.SettingsManagement;
using pdfforge.PDFCreator.Core.Workflow;
using Prism.Events;
using SimpleInjector;

namespace pdfforge.PDFCreator.UI.COM;

public class ComDependencyBuilder
{
    private readonly ComBaseBootstrapper _comBootstrapper;
    private static ComDependencies _comDependencies;


    public ComDependencies ComDependencies()
    {
        if (_comDependencies != null)
            return _comDependencies;

        return _comDependencies = BuildComDependencies();

    }

    public Action<Container> ModifyRegistrations { get; set; } = container => { };

    public static void ResetDependencies()
    {
        _comDependencies = null;
    }

    public ComDependencyBuilder(ComBaseBootstrapper comBootstrapper)
    {
        _comBootstrapper = comBootstrapper;
    }

    public ComDependencies BuildComDependencies()
    {
        var container = new Container();
        container.Options.ResolveUnregisteredConcreteTypes = true;
        container.Options.EnableAutoVerification = false;
        _comBootstrapper.ConfigureContainer(container);
        container.Register<PrintFileHelperComFactory>();
        container.Register<PdfCreatorAdapter>();
        container.Register<QueueAdapter>();
        container.Register<ComDependencies>();
        container.RegisterSingleton(() => new ThreadPool());
        container.Register<IPrintJobAdapterFactory, PrintJobAdapterFactory>();
        container.Register<IEventAggregator, EventAggregator>();

        container.RegisterInitializer<IJobInfoQueueManager>(manager => manager.AutoStartProcessing = false);

        DoModifyRegistrations(container);

        LoggingHelper.InitFileLogger("PDFCreator", LoggingLevel.Error);

        var dependencies = container.GetInstance<ComDependencies>();

        var settingsManager = container.GetInstance<ISettingsManager>();
        settingsManager.LoadAllSettings();

        var translator = container.GetInstance<ITranslationHelper>();
        translator.InitTranslator("english");

        _comBootstrapper.InitializeServices(container);

        return dependencies;
    }

    private void DoModifyRegistrations(Container container)
    {
        container.Options.AllowOverridingRegistrations = true;

        ModifyRegistrations(container);

        container.Options.AllowOverridingRegistrations = false;
    }
}
