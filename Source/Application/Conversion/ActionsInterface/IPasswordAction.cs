using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Conversion.ActionsInterface;

public interface IPasswordAction : IAction
{
    void SetPassword(Job job, string password);

    string GetMissingPasswordErrorText();
}
