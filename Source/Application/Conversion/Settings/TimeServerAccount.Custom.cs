using pdfforge.DataStorage;

namespace pdfforge.PDFCreator.Conversion.Settings;

public partial class TimeServerAccount
{
    public string AccountInfo => $"{Url}";

    public void CopyTo(TimeServerAccount targetAccount)
    {
        var data = Data.CreateDataStorage();
        StoreValues(data, "");
        targetAccount.ReadValues(data, "");
    }
}
