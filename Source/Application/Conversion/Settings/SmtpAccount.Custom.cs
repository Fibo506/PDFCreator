using pdfforge.DataStorage;

namespace pdfforge.PDFCreator.Conversion.Settings;

public partial class SmtpAccount
{
    public string AccountInfo => Address;

    public void CopyTo(SmtpAccount targetAccount)
    {
        var data = Data.CreateDataStorage();
        StoreValues(data, "");
        targetAccount.ReadValues(data, "");
    }
}
