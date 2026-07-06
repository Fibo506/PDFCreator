using System;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Conversion.Settings.GroupPolicies;
using pdfforge.PDFCreator.Core.SettingsManagementInterface;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Threading;
using pdfforge.UsageStatistics;

namespace pdfforge.PDFCreator.Core.UsageStatistics;

public abstract class UsageStatisticsManagerBase
{
    private readonly IUsageStatisticsSender _sender;
    private readonly IOsHelper _osHelper;
    protected readonly IUsageMetricFactory UsageMetricFactory;
    private readonly ISettingsProvider _settingsProvider;
    protected readonly IGpoSettings GpoSettings;
    private readonly IThreadManager _threadManager;

    protected UsageStatisticsManagerBase(IUsageStatisticsSender sender, IOsHelper osHelper,
        IUsageMetricFactory usageMetricFactory, ISettingsProvider settingsProvider, IGpoSettings gpoSettings, IThreadManager threadManager)
    {
        _sender = sender;
        _osHelper = osHelper;
        UsageMetricFactory = usageMetricFactory;
        _settingsProvider = settingsProvider;
        GpoSettings = gpoSettings;
        _threadManager = threadManager;
    }

    protected bool IsEnabled => !GpoSettings.DisableUsageStatistics && _settingsProvider.Settings.ApplicationSettings.UsageStatistics.Enable;

    protected string OperatingSystem => _osHelper.GetWindowsVersion();

    protected void DoSendUsageStatistics(Func<IUsageMetric> createStatisticsMetric)
    {
        try
        {
            if (IsEnabled)
            {
                var usageMetric = createStatisticsMetric();

                var senderThread = new SynchronizedThread(() => _sender.Send(usageMetric));

                _threadManager.StartSynchronizedThread(senderThread);
            }
        }
        catch (Exception)
        { }
    }
}

public class PdfCreatorUsageStatisticsManager : UsageStatisticsManagerBase, IPdfCreatorUsageStatisticsManager
{
    public PdfCreatorUsageStatisticsManager(IUsageStatisticsSender sender, IOsHelper osHelper,
        IUsageMetricFactory usageMetricFactory,
        ISettingsProvider settingsProvider, IGpoSettings gpoSettings, IThreadManager threadManager) :
        base(sender, osHelper, usageMetricFactory, settingsProvider, gpoSettings, threadManager)
    { }


    public void SendUsageStatistics(TimeSpan duration, Job job, string status)
    {
        DoSendUsageStatistics(() => CreateJobUsageStatisticsMetric(job, duration, status));
    }

    public bool IsComMode { get; set; }

    private Mode GetMode(bool autoSave)
    {
        if (IsComMode)
            return Mode.Com;

        return autoSave ? Mode.AutoSave : Mode.Interactive;
    }

    private PdfCreatorJobFinishedMetric CreateJobUsageStatisticsMetric(Job job, TimeSpan duration, string status)
    {
        var metric = UsageMetricFactory.CreateMetric<PdfCreatorJobFinishedMetric>();

        metric.OperatingSystem = OperatingSystem;

        //Job
        metric.OutputFormat = job.Profile.OutputFormat.ToString();
        metric.Mode = GetMode(job.Profile.AutoSave.Enabled);
        metric.SaveFileTemporarily = job.Profile.SaveFileTemporary;
        metric.AutoSaveExistingFileBehaviour = job.Profile.AutoSave.ExistingFileBehaviour.ToString();
        metric.SkipSendFailures = job.Profile.SkipSendFailures;
        metric.QuickActions = job.Profile.ShowQuickActions;
        metric.ShowTrayNotifications = job.Profile.ShowAllNotifications;

        //Result
        metric.TotalPages = job.JobInfo.TotalPages;
        metric.NumberOfCopies = job.NumberOfCopies;
        metric.Duration = (long)duration.TotalMilliseconds;
        metric.Status = status;

        //Preparation Actions
        metric.CustomScript = job.Profile.CustomScript.Enabled;
        metric.UserToken = job.Profile.UserTokens.Enabled;
        metric.ForwardToFurtherProfile = job.Profile.ForwardToFurtherProfile.Enabled;

        //Modify Actions
        metric.Cover = job.Profile.CoverPage.Enabled;
        metric.Attachment = job.Profile.AttachmentPage.Enabled;
        metric.Stamp = job.Profile.Stamping.Enabled;
        metric.Watermark = job.Profile.Watermark.Enabled;
        metric.Background = job.Profile.BackgroundPage.Enabled;
        metric.PageNumbers = job.Profile.PageNumbers.Enabled;
        metric.Encryption = job.Profile.PdfSettings.Security.Enabled;
        metric.Signature = job.Profile.PdfSettings.Signature.Enabled;
        metric.DisplaySignatureInDocument = job.Profile.PdfSettings.Signature.Enabled
                                         && job.Profile.PdfSettings.Signature.DisplaySignature != DisplaySignature.NoDisplay;

        //Send actions
        metric.Mailclient = job.Profile.EmailClientSettings.Enabled;
        metric.MailWebAccess = job.Profile.EmailWebSettings.Enabled;
        metric.MailWebAccessSendMailAutomatically = job.Profile.EmailWebSettings.SendWebMailAutomatically;
        metric.Smtp = job.Profile.EmailSmtpSettings.Enabled;
        metric.SmtpSendOnBehalfOf = !string.IsNullOrWhiteSpace(job.Profile.EmailSmtpSettings.OnBehalfOf);
        metric.SmtpReplyTo = !string.IsNullOrWhiteSpace(job.Profile.EmailSmtpSettings.ReplyTo);
        metric.OpenViewer = job.Profile.OpenViewer.Enabled;
        metric.OpenWithPdfArchitect = job.Profile.OpenViewer.OpenWithPdfArchitect;
        metric.Script = job.Profile.Scripting.Enabled;
        metric.Print = job.Profile.Printing.Enabled;
        metric.Ftp = job.Profile.Ftp.Enabled;
        metric.Http = job.Profile.HttpSettings.Enabled;
        metric.OneDrive = job.Profile.OneDriveSettings.Enabled;
        metric.OneDriveShareLink = job.Profile.OneDriveSettings.CreateShareLink;
        metric.Dropbox = job.Profile.DropboxSettings.Enabled;
        metric.Sharepoint = job.Profile.SharepointSettings.Enabled;
        metric.SharepointShareLink = job.Profile.SharepointSettings.CreateShareLink;

        //GPOs
        metric.DisableApplicationSettings = GpoSettings.DisableApplicationSettings;
        metric.DisableDebugTab = GpoSettings.DisableDebugTab;
        metric.DisablePrinterTab = GpoSettings.DisablePrinterTab;
        metric.DisableProfileManagement = GpoSettings.DisableProfileManagement;
        metric.DisableTitleTab = GpoSettings.DisableTitleTab;
        metric.DisableHistory = GpoSettings.DisableHistory;
        metric.DisableAccountsTab = GpoSettings.DisableAccountsTab;
        metric.DisableRssFeed = GpoSettings.DisableRssFeed;
        metric.DisableTips = GpoSettings.DisableTips;
        metric.HideLicenseTab = GpoSettings.HideLicenseTab;
        metric.HidePdfArchitectInfo = GpoSettings.HidePdfArchitectInfo;
        metric.GpoLanguage = GpoSettings.Language ?? "";
        metric.DisableLicenseExpirationReminder = GpoSettings.DisableLicenseExpirationReminder;
        metric.GpoUpdateInterval = GpoSettings.UpdateInterval ?? "";
        metric.GpoHotStandbyMinutes = GpoSettings.HotStandbyMinutes;

        //Share settings
        metric.LoadSharedAppSettings = GpoSettings.LoadSharedAppSettings;
        metric.LoadSharedProfiles = GpoSettings.LoadSharedProfiles;
        metric.LoadSharedPrinterMappings = GpoSettings.LoadSharedPrinterMappings;
        metric.IsShared = job.Profile.Properties.IsShared;
        metric.AllowUserDefinedProfiles = GpoSettings.AllowUserDefinedProfiles;
        metric.HasShareFilename = GpoSettings.SharedSettingsFilename != "settings";

        return metric;
    }
}
