using Codeuctivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using SystemInterface.IO;
using Formatting = Newtonsoft.Json.Formatting;
using Job = pdfforge.PDFCreator.Conversion.Jobs.Jobs.Job;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfAValidation;

public interface IPdfAValidator
{
    void WriteValidationReport(Job job);
}

public class PdfAValidator : IPdfAValidator
{
    private readonly IFile _file;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public PdfAValidator(IFile file)
    {
        _file = file;
    }

    public void WriteValidationReport(Job job)
    {
        var pdfAFile = job.OutputFiles.First();

        var pdfAValidationReport = new PdfAValidationReport()
        {
            document = PathSafe.GetFileName(pdfAFile),
            validation_date = DateTime.Now,
            validation_level = job.Profile.OutputFormat.GetDescription(),
            validator = job.Producer
        };

        using (var validator = new Codeuctivity.PdfAValidator())
        {
            var result = validator.ValidateWithDetailedReportAsync(pdfAFile).GetAwaiter().GetResult();
            var validationReport = result.Jobs.Job.ValidationReport;
            pdfAValidationReport.is_valid = validationReport.IsCompliant;
            pdfAValidationReport.validation_errors = CollectValidationErrorResult(validationReport.Details);
        }

        WriteValidationReport(pdfAFile, pdfAValidationReport);
    }

    private ValidationErrorResult CollectValidationErrorResult(Details reportDetails)
    {
        var validationResult = new ValidationErrorResult();
        validationResult.failed_rules = reportDetails.FailedRules;
        validationResult.failed_checks = reportDetails.FailedChecks;
        validationResult.rules = reportDetails.Rule;
        _logger.Info("Passed rules " + reportDetails.PassedRules);
        _logger.Info("Passed checks " + reportDetails.FailedChecks);
        return validationResult;
    }

    private void WriteValidationReport(string pdfAFile, PdfAValidationReport pdfAValidationReport)
    {
        var path = PathSafe.ChangeExtension(pdfAFile, "report.json");
        try
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            var jsonObj = JsonConvert.SerializeObject(pdfAValidationReport, jsonSettings);

            _file.WriteAllText(path, jsonObj);
        }
        catch (Exception ex)
        {
            throw new ProcessingException(ex.GetType() + " during PDF/A conversion:" + Environment.NewLine + ex.Message, ErrorCode.Conversion_PdfAError, ex);
        }
    }
}
