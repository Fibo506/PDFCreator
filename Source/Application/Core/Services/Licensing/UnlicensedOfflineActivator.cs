using Optional;
using pdfforge.LicenseValidator.Interface;
using pdfforge.LicenseValidator.Interface.Data;

namespace pdfforge.PDFCreator.Core.Services.Licensing;

public class UnlicensedOfflineActivator : IOfflineActivator
{
    public string BuildOfflineActivationString(string key)
    {
        return "";
    }

    public Option<Activation, LicenseError> ActivateOfflineActivationString(string encodedOfflineActivationString)
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public Option<Activation, LicenseError> ValidateOfflineActivationString(string encodedOfflineActivationString)
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public void SaveActivation(Activation activation)
    {
    }
}
