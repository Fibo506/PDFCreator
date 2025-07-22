using Optional;
using pdfforge.LicenseValidator.Interface.Data;

namespace pdfforge.PDFCreator.Utilities.License;

public interface ILicenseHelper
{
    void InformLicenseInteraction(Option<Activation, LicenseError> activation);
}
