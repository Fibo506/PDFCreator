using System.Globalization;

namespace pdfforge.PDFCreator.Core.Services;

public class Language
{
    public string CommonName { get; set; }
    public string NativeName { get; set; }
    public string Iso2 { get; set; }
    public CultureInfo CultureInfo { get; set; }
}
