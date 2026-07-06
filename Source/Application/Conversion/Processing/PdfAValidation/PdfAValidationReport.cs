using System.Diagnostics.CodeAnalysis;
using Codeuctivity;
using Newtonsoft.Json;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfAValidation;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class PdfAValidationReport
{

    public bool is_valid { get; set; }
    public string document { get; set; }
    public DateTime validation_date { get; set; }
    public string validation_level { get; set; }
    public string validator { get; set; }
    public ValidationErrorResult validation_errors { get; set; } = new();
    
}

public class ValidationErrorResult
{
    public int failed_rules;
    public int failed_checks;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Rule> rules;
}
