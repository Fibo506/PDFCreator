using System;
using System.Linq;
using System.Runtime.InteropServices;
using pdfforge.CustomScriptAction;
using pdfforge.DataStorage;
using pdfforge.Mail;
using pdfforge.PDFCreator.Conversion.Actions;
using pdfforge.PDFCreator.Conversion.Actions.Actions;
using pdfforge.PDFCreator.Conversion.Actions.Actions.Dropbox;
using pdfforge.PDFCreator.Conversion.Actions.Actions.Ftp;
using pdfforge.PDFCreator.Conversion.Actions.Actions.Interface;
using pdfforge.PDFCreator.Conversion.Actions.Actions.Mail;
using pdfforge.PDFCreator.Conversion.Actions.Actions.OneDrive;
using pdfforge.PDFCreator.Conversion.Actions.Queries;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.ConverterInterface;
using pdfforge.PDFCreator.Conversion.Dropbox;
using pdfforge.PDFCreator.Conversion.Ghostscript;
using pdfforge.PDFCreator.Conversion.Ghostscript.Conversion;
using pdfforge.PDFCreator.Conversion.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.FolderProvider;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Conversion.Processing.PdfAValidation;
using pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.Core.ComImplementation;
using pdfforge.PDFCreator.Core.Communication;
using pdfforge.PDFCreator.Core.Controller;
using pdfforge.PDFCreator.Core.Controller.Routing;
using pdfforge.PDFCreator.Core.DirectConversion;
using pdfforge.PDFCreator.Core.JobInfoQueue;
using pdfforge.PDFCreator.Core.Printing;
using pdfforge.PDFCreator.Core.Printing.Port;
using pdfforge.PDFCreator.Core.Printing.Printer;
using pdfforge.PDFCreator.Core.Printing.Printing;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.Core.Services.JobEvents;
using pdfforge.PDFCreator.Core.Services.JobHistory;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.Core.Services.Trial;
using pdfforge.PDFCreator.Core.Services.Update;
using pdfforge.PDFCreator.Core.SettingsManagement;
using pdfforge.PDFCreator.Core.SettingsManagement.DefaultSettings;
using pdfforge.PDFCreator.Core.SettingsManagement.GPO;
using pdfforge.PDFCreator.Core.SettingsManagement.GPO.Settings;
using pdfforge.PDFCreator.Core.SettingsManagement.Helper;
using pdfforge.PDFCreator.Core.SettingsManagement.SettingsLoading;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;
using pdfforge.PDFCreator.Core.Startup;
using pdfforge.PDFCreator.Core.Startup.StartConditions;
using pdfforge.PDFCreator.Core.StartupInterface;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.Core.Workflow.ComposeTargetFilePath;
using pdfforge.PDFCreator.Core.Workflow.Exceptions;
using pdfforge.PDFCreator.Core.Workflow.Output;
using pdfforge.PDFCreator.ErrorReport;
using pdfforge.PDFCreator.UI.COM.Helper;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.IO;
using pdfforge.PDFCreator.Utilities.License;
using pdfforge.PDFCreator.Utilities.Messages;
using pdfforge.PDFCreator.Utilities.Registry;
using pdfforge.PDFCreator.Utilities.Spool;
using pdfforge.PDFCreator.Utilities.Threading;
using pdfforge.PDFCreator.Utilities.Tokens;
using pdfforge.PDFCreator.Utilities.Update;
using pdfforge.PDFCreator.Utilities.Web;
using pdfforge.UsageStatistics;
using SimpleInjector;
using SystemInterface;
using SystemInterface.Diagnostics;
using SystemInterface.IO;
using SystemWrapper;
using SystemWrapper.Diagnostics;
using SystemWrapper.IO;
using Translatable;
using static pdfforge.PDFCreator.Core.Workflow.PdfToPreviewConverter;
using HashUtil = pdfforge.PDFCreator.Utilities.HashUtil;
using IHashUtil = pdfforge.PDFCreator.Utilities.IHashUtil;
using IProcessStarter = pdfforge.PDFCreator.Utilities.Process.IProcessStarter;
using IRegistry = SystemInterface.Microsoft.Win32.IRegistry;
using IWebLinkLauncher = pdfforge.PDFCreator.Utilities.Web.IWebLinkLauncher;
using ProcessStarter = pdfforge.PDFCreator.Utilities.Process.ProcessStarter;
using RegistryWrap = SystemWrapper.Microsoft.Win32.RegistryWrap;
using WebLinkLauncher = pdfforge.PDFCreator.Utilities.Web.WebLinkLauncher;

namespace pdfforge.PDFCreator.UI.COM;

[ComVisible(false)]
public abstract class ComBaseBootstrapper
{
    [Obsolete]
    public void ConfigureContainer(Container container)
    {
        container.RegisterSingleton<ApplicationNameProvider>(() => new ApplicationNameProvider("PDFCreatorCOM"));

        RegisterGhostscript(container);
        RegisterPrinter(container);

        container.RegisterSingleton<IInstallationPathProvider>(() => InstallationPathProviders.PDFCreatorProvider);

        container.RegisterSingleton<DropboxAppData>(() => new DropboxAppData(Data.Decrypt(DropboxAppKey.Encrypted_DropboxAppKey)));

        container.RegisterSingleton<IPrinterPortReader, PrinterPortReader>();
        container.RegisterInstance<IVersionHelper>(new VersionHelper(GetType().Assembly));
        container.RegisterSingleton<IFolderCleaner, FolderCleaner>();
        container.RegisterSingleton<IPdfCreatorFolderCleanUp, PdfCreatorFolderCleanUp>();
        container.RegisterSingleton<IDefaultSettingsBuilder, ComDefaultSettingsBuilder>();
        container.RegisterSingleton<IJobBuilder, JobBuilderProfessional>();
        container.RegisterSingleton<IHashUtil, HashUtil>();
        container.RegisterSingleton<ISignaturePasswordCheck, SignaturePasswordCheckCached>();
        container.RegisterSingleton<ICancellationTokenSourceFactory, CancellationTokenSourceFactory>();
        container.RegisterSingleton<IFontHelper, FontHelper>();

        container.RegisterSingleton<ISettingsBackup, SettingsBackup>();
        container.RegisterSingleton<IMigrationStorageFactory>(() =>
            new MigrationStorageFactory((baseStorage, targetVersion, settingsBackup) => new CreatorSettingsMigrationStorage(baseStorage, container.GetInstance<IFontHelper>(), targetVersion, settingsBackup, container.GetInstance<IGuid>())));

        container.RegisterSingleton<IJobInfoManager, JobInfoManager>();
        container.Register<IConversionWorkflow, AutoSaveWorkflow>();
        container.Register<ITargetFilePathComposer, TargetFilePathComposer>();
        container.Register<IDirectoryHelper, DirectoryHelper>();
        container.RegisterSingleton<INumberOfPagesHelper, NumberOfPagesHelper>();
        container.Register<ILastSaveDirectoryHelper, LastSaveDirectoryHelper>();

        container.RegisterSingleton<IAssemblyHelper>(() => new AssemblyHelper(GetType().Assembly));
        container.RegisterSingleton<IProgramDataDirectoryHelper>(() => new ProgramDataDirectoryHelper("PDFCreator Server"));
        container.RegisterSingleton<IPathUtil, PathUtil>();
        container.RegisterSingleton<IFileIndexHelper, FileIndexHelper>();
        container.RegisterSingleton<IFileVersionInfoHelper, FileVersionInfoHelper>();
        container.RegisterSingleton<IProfileChecker, ProfileChecker>();

        container.RegisterSingleton<IDirectConversion, DirectConversion>();
        container.RegisterSingleton<IDirectConversionHelper, DirectConversionHelper>();

        container.RegisterSingleton<IDirectConversionInfFileHelper, DirectConversionInfFileHelper>();
        container.RegisterSingleton<IDirectImageConversionHelper, DirectImageConversionHelper>();
        container.RegisterSingleton<ITestPageCreator, TestPageCreator>();
        container.RegisterSingleton<IRecommendArchitectAssistant, ComRecommendArchitectAssistant>();

        container.Register<IJobRunner, JobRunner>();
        container.Register<IMergeBeforeActionsHelper, MergeBeforeActionsHelper>();
        container.Register<IActionExecutor, ActionExecutor>();
        container.Register<IOutputFileMover, AutosaveOutputFileMover>();
        container.Register<ITokenReplacerFactory, TokenReplacerFactory>();

        container.Register<IDateTimeProvider, DateTimeProvider>();

        container.Register<ISplitDocumentFilePathHelper, SplitDocumentFilePathHelper>();

        container.Register<INotificationService, DisabledNotificationService>();

        container.Register<IFailedJobHandler, BaseFailedJobHandler>();
        container.Register<IRepairSpoolFolderAssistant, RepairSpoolFolderAssistant>();
        container.Register<IShellExecuteHelper, ShellExecuteHelper>();
        container.Register<IRepairPrinterAssistant, RepairPrinterAssistant>();

        container.RegisterSingleton<IPDFCreatorNameProvider, PDFCreatorNameProvider>();
        container.RegisterSingleton<IFontPathHelper, FontPathHelper>();
        container.RegisterSingleton<ILicenseHelper, ComLicenseHelper>();
        container.RegisterSingleton<ICampaignHelper, CampaignHelper>();
        container.RegisterSingleton<IMessageHelper, ComMessageHelper>();
        container.RegisterSingleton<IMainWindowThreadLauncher, ComMainWindowThreadLauncher>();

        container.RegisterSingleton<IOsHelper, OsHelper>();

        container.RegisterSingleton<IConverterFactory, ConverterFactory>();
        container.RegisterSingleton<IPsConverterFactory, GhostscriptConverterFactory>();

        container.RegisterSingleton<IJobInfoQueueManager, JobInfoQueueManager>();
        container.Register<IJobInfoQueue, JobInfoQueue>(Lifestyle.Singleton);
        container.Register<IThreadManager, ThreadManager>(Lifestyle.Singleton);
        container.RegisterSingleton<IFileAssoc, FileAssoc>();
        container.RegisterSingleton<IJobCleanUp, JobCleanUp>();
        container.Register<IJobDataUpdater, JobDataUpdater>();
        container.RegisterSingleton<IJobInfoDuplicator, JobInfoDuplicator>();
        container.RegisterSingleton<ISourceFileInfoDuplicator, SourceFileInfoDuplicator>();
        container.RegisterSingleton<IJobFolderBuilder, JobFolderBuilder>();
        container.RegisterSingleton<IGpoSettings>(GetGpoSettings);

        container.RegisterSingleton<UpdateInformationProvider>(() => new UpdateInformationProvider(Urls.PdfCreatorServerUpdateInfoUrl, "PDFCreatorServer", Urls.PdfCreatorServerUpdateChangelogUrl));

        container.Register<IPageNumberCalculator, PageNumberCalculator>();

        container.RegisterSingleton<IPsToPdfConverter, PsToPdfConverter>();

        container.Register<IFtpConnectionFactory, FtpConnectionFactory>();
        container.Register<IFtpConnectionTester, FtpConnectionTester>();

        container.Register<IProcessStarter, ProcessStarter>();
        container.RegisterSingleton<IUpdateDownloader, UpdateDownloader>();
        container.RegisterSingleton<ISettingsManager, ComSettingsManager>();
        container.RegisterSingleton<IUpdateHelper, DisabledUpdateHelper>();
        container.RegisterSingleton<ISettingsLoader, ComSettingsLoader>();
        container.RegisterSingleton<ISettingsMover, SettingsMover>();
        container.RegisterSingleton<IRegistryUtility, RegistryUtility>();
        container.RegisterSingleton<IPdfArchitectCheck, PdfArchitectCheck>();
        container.RegisterSingleton<IManagePrintJobExceptionHandler, ComManagePrintJobExceptionHandler>();

        container.Register<IMailSignatureHelper, ComMailSignatureHelper>();
        container.Register<IEmailClientFactory, EmailClientFactory>();
        container.Register<IDefaultViewerCheck, DefaultViewerCheck>();
        container.Register<ComStartupConditionChecker>();

        container.Register<IJobPrinter, GhostScriptPrinter>();

        container.RegisterSingleton<IComWorkflowFactory>(() => new ComWorkflowFactory(container));

        container.RegisterSingleton<IStoredParametersManager, StoredParametersManager>();
        container.RegisterSingleton<IDataStorageFactory, DataStorageFactory>();

        container.RegisterSingleton<ITitleReplacerProvider, SettingsTitleReplacerProvider>();
        container.RegisterSingleton<ISpoolFolderAccess, SpoolFolderAccess>();

        container.Register<IDropboxService, DropboxService>();
        container.RegisterSingleton<IJobInfoToProfileMapper, JobInfoToProfileMapper>();
        container.RegisterSingleton<IDropboxTokenCache, DropboxTokenCache>();

        container.Register<ICustomScriptHandler, CustomScriptHandler>();
        container.RegisterSingleton<ICustomScriptLoader, CsScriptLoader>();

        container.RegisterSingleton<IUniqueFilenameFactory, UniqueFilenameFactory>();
        container.RegisterSingleton<IUniqueDirectory, UniqueDirectory>();
        container.RegisterSingleton<IJobHistoryActiveRecord, JobHistoryActiveRecord>();
        container.RegisterSingleton<IJobHistoryStorage, JobHistoryJsonFileStorage>();
        container.RegisterSingleton<IApplicationCloser, ComApplicationCloser>();
        container.RegisterSingleton<IJobEventsManager, JobEventsManager>();
        container.RegisterSingleton<IPreviewPreLoadHelper, DisabledPreviewPreloadHelper>();
        container.RegisterSingleton<IGraphManager, GraphManager>();
        container.Register<IActionManager, ActionManager>();
        IWorkflowFactory workflowFactory = new ComWorkflowFactory(container);
        container.RegisterSingleton<IWorkflowFactory>(() => workflowFactory);

        container.Collection.Register<IJobEventsHandler>(new IJobEventsHandler[]
        {
        });

        container.RegisterSingleton<IAppSettingsProvider>(() =>
        {
            var settings = container.GetInstance<ISettingsProvider>();
            return new ApplicationSettingsProvider(() => settings.Settings.ApplicationSettings);
        });

        container.Register<IMailHelper, MailHelper>();
        container.Register<ITestFileDummyHelper, TestFileDummyHelper>();
        container.RegisterSingleton<ITempDirectoryHelper, TempDirectoryHelper>();
        container.RegisterSingleton(() => new ErrorHelper("pdfcreator_com", "PDFCreator Com", container.GetInstance<IVersionHelper>().ApplicationVersion, Urls.SentryDsnUrl));
        container.RegisterSingleton<IPreviewManager, PreviewManager>();
        container.RegisterSingleton(() => new EditionHelper(Edition.Server));
        container.Register<IPreviewChangesHelper, DisabledPreviewChangesHelper>();
        container.RegisterSingleton<IPdfToPreviewConverter, DisabledPdfToPreviewConverter>();
        container.RegisterSingleton<IJobCleaner, JobCleaner>();
        container.RegisterSingleton<IPdfAValidator, PdfAValidator>();
        container.RegisterSingleton<IGuid, GuidWrap>();

        container.Register<IPrinterWrapper, PrinterWrapper>();
        container.RegisterSingleton<ICommandLineUtil, CommandLineUtil>();
        container.Register<PrintingDeviceFactory>();

        container.Register<IPipeServerManager, PipeServerManager>(Lifestyle.Singleton);
        container.RegisterSingleton<IFileConversionHelper, FileConversionHelper>();
        container.RegisterSingleton<IPrintFileHelper, ComPrintFileHelper>();
        container.Register<IPipeMessageHandler, NewPipeJobHandler>(Lifestyle.Singleton);
        container.RegisterInitializer<PrintingDeviceFactory>(pdb => pdb.Init(true));

        container.Register<IEncryptedDocumentHelper, DisabledEncryptedDocumentHelper>();
        container.RegisterInstance<IStartupArgs>(new StartupArgs() { Args = [] });
        container.RegisterSingleton<IWebLinkHandler, InactiveWebLinkHandler>();

        RegisterEditionSpecificPackages(container);
        RegisterActionFacade(container);
        RegisterLicensing(container);
        RegisterSystemInterfaces(container);
        RegisterActions(container);
        RegisterFolderProvider(container);
        RegisterUsageStatistics(container);
        RegisterSettingsHelper(container);
        RegisterTranslator(container);
        RegisterStartupConditions(container);

        RegisterAdditionalDependencies(container);
        RegisterWebLinkLauncher(container);
    }

    protected virtual void RegisterEditionSpecificPackages(Container container)
    {

    }

    protected virtual Type[] GetStartupConditions()
    {
        return [];
    }

    private void RegisterStartupConditions(Container container)
    {
        var defaultConditions = GetStartupConditions().ToList();

        container.Collection.Register<IStartupCondition>(defaultConditions);
        container.Register<ICheckAllStartupConditions, CheckAllStartupConditions>();

    }

    private void RegisterSettingsHelper(Container container)
    {

        // Register the same SettingsHelper for SettingsHelper and ISettingsProvider
        var registration = Lifestyle.Singleton.CreateRegistration(CreateSettingsProvider, container);
        container.AddRegistration(typeof(SettingsProvider), registration);
        container.AddRegistration(typeof(ISettingsProvider), registration);
        container.AddRegistration(typeof(IApplicationLanguageProvider), registration);
    }

    protected SettingsProvider CreateSettingsProvider()
    {
        return new GpoAwareSettingsProvider(GetGpoSettings());
    }

    private void RegisterTranslator(Container container)
    {
        var registration = Lifestyle.Singleton.CreateRegistration<TranslationHelper>(container);
        container.AddRegistration(typeof(BaseTranslationHelper), registration);
        container.AddRegistration(typeof(TranslationHelper), registration);
        container.AddRegistration(typeof(ILanguageProvider), registration);
        container.AddRegistration(typeof(ITranslationHelper), registration);

        var translationFactory = new TranslationFactory();
        container.RegisterSingleton(() => translationFactory);
        var cachedTranslationFactory = new CachedTranslationFactory(translationFactory);
        registration = Lifestyle.Singleton.CreateRegistration(() => cachedTranslationFactory, container);
        container.AddRegistration(typeof(CachedTranslationFactory), registration);
        container.AddRegistration(typeof(ITranslationFactory), registration);

        container.RegisterSingleton<ISharedSettingsLoader, SharedSettingsLoader>();
        container.RegisterSingleton<IBaseSettingsBuilder, DefaultBaseSettingsBuilder>();
        container.RegisterSingleton<IIniSettingsLoader, IniSettingsLoader>();
        container.RegisterInitializer<IIniSettingsLoader>(loader => loader.SettingsVersion = (new Conversion.Settings.CreatorAppSettings()).SettingsVersion);

    }

    protected IGpoSettings GetGpoSettings()
    {
        var gpoReader = new GpoReader<GpoSettings>(@"pdfforge\PDFCreator\Settings");
        return new GpoReaderSettings(gpoReader.ReadGpoSettings(new GpoSettings()));
        // use this to test GPO-restrictions by modifying GpoReaderDebugSettings
        // return new GpoReaderDebugSettings();
    }

    private void RegisterWebLinkLauncher(Container container)
    {
        var installationPathProvider = InstallationPathProviders.PDFCreatorServerProvider;
        container.Register(() => TrackingParameterReader.ReadFromRegistry(installationPathProvider.ApplicationRegistryPath));
        container.Register<IWebLinkLauncher, WebLinkLauncher>();
    }


    private void RegisterUsageStatistics(Container container)
    {
        container.RegisterSingleton<UsageStatisticsOptions>(() =>
        {
            var version = container.GetInstance<IVersionHelper>()
                .FormatWithThreeDigits();

            var product = container.GetInstance<ApplicationNameProvider>().ProductIdentifier;

            return new UsageStatisticsOptions(Urls.UsageStatisticsEndpointUrl, product, version);
        });
        container.RegisterSingleton<IUsageMetricFactory, UsageMetricFactory>();
    }

    protected virtual void RegisterActionFacade(Container container)
    {
    }

    protected virtual void RegisterAdditionalDependencies(Container container)
    {
    }

    protected virtual void RegisterLicensing(Container container)
    {
    }

    private void RegisterGhostscript(Container container)
    {
        container.RegisterSingleton<IGhostscriptDiscovery, GhostscriptDiscovery>();
        container.RegisterInitializer<GhostscriptDiscovery>(gs =>
        {
            gs.PossibleGhostscriptPaths.Add(@"..\..\packages\test\Ghostscript");
            gs.PossibleGhostscriptPaths.Add(@"..\..\..\packages\test\Ghostscript");
            gs.PossibleGhostscriptPaths.Add(@"..\..\..\..\packages\test\Ghostscript");
            gs.PossibleGhostscriptPaths.Add(@"..\..\..\..\..\packages\test\Ghostscript");
        });
    }


    private void RegisterActions(Container container)
    {

        //Register Actions in default order
        container.Collection.Register<IAction>(new[]
        {
            // preparation
            typeof(UserTokensAction),
            typeof(PreConversionScriptAction),

            // modification
            typeof(CoverAction),
            typeof(AttachmentAction),
            typeof(BackgroundAction),
            typeof(WatermarkAction),
            typeof(StampAction),
            typeof(PageNumbersAction),
            typeof(EncryptionAction),
            typeof(SigningAction),
            typeof(PostConversionScriptAction),

            // sending
            typeof(OpenFileAction),
            typeof(ScriptAction),
            typeof(PrintingAction),
            typeof(MailClientAction),
            typeof(MailWebAction),
            typeof(SmtpMailAction),
            typeof(DropboxAction),
            typeof(OneDriveAction),
            typeof(FtpAction),
            typeof(HttpAction),
        });

        container.RegisterSingleton<IActionOrderHelper, ActionOrderHelper>();

        //container.Register<IActionLocator, ActionLocator>();

        container.Register<ISmtpMailAction, SmtpMailAction>();
        container.Register<IEMailClientAction, MailClientAction>();
        container.Register<IOpenFileAction, OpenFileAction>();
        container.Register<IHttpAction, HttpAction>();
    }

    private void RegisterPrinter(Container container)
    {
        var registration = Lifestyle.Singleton.CreateRegistration<PrinterHelper>(container);
        container.AddRegistration(typeof(IPrinterHelper), registration);
        container.AddRegistration(typeof(IPrinterProvider), registration);

        container.RegisterSingleton<ISystemPrinterProvider, SystemPrinterProvider>();
    }

    private void RegisterFolderProvider(Container container)
    {
        var registration = Lifestyle.Singleton.CreateRegistration<FolderProvider>(container);

        container.AddRegistration(typeof(ITempFolderProvider), registration);
        container.AddRegistration(typeof(ISpoolerProvider), registration);
        container.AddRegistration(typeof(IAppDataProvider), registration);
    }

    private void RegisterSystemInterfaces(Container container)
    {

        container.RegisterSingleton<IDirectory, DirectoryWrap>();
        container.RegisterSingleton<IFile, FileWrap>();
        container.RegisterSingleton<IRegistry, RegistryWrap>();
        container.Register<IProcess, ProcessWrap>();
        container.RegisterSingleton<IPath, PathWrap>();
        container.RegisterSingleton<IEnvironment, EnvironmentWrap>();
        container.RegisterSingleton<IDirectoryAccessControl, DirectoryAccessControl>();
    }

    public virtual void InitializeServices(Container container)
    {

    }
}
