namespace pdfforge.PDFCreator.Conversion.Settings;

public class JobPasswords
{
    public string PdfUserPassword { get; set; }
    public string PdfOwnerPassword { get; set; }
    public string PdfSignaturePassword { get; set; }
    public string SmtpPassword { get; set; }
    public string FtpPassword { get; set; }
    public string HttpPassword { get; set; }
}
