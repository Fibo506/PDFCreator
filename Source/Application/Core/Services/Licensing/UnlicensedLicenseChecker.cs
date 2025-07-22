using Optional;
using pdfforge.LicenseValidator.Interface;
using pdfforge.LicenseValidator.Interface.Data;

namespace pdfforge.PDFCreator.Core.Services.Licensing;

public class UnlicensedLicenseChecker : ILicenseChecker
{
    public Option<Activation, LicenseError> GetSavedActivation()
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public Option<Activation, LicenseError> GetActivation()
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public Option<string, LicenseError> GetSavedLicenseKey()
    {
        return Option.None<string, LicenseError>(LicenseError.UnknownError);
    }

    public Option<Activation, LicenseError> ActivateWithKey(string key)
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public Option<Activation, LicenseError> ActivateWithoutSaving(string key)
    {
        return Option.None<Activation, LicenseError>(LicenseError.UnknownError);
    }

    public void SaveActivation(Activation activation)
    {
    }
}
