using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.PrintJob;
using pdfforge.PDFCreator.Utilities.Threading;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime;

public class DesignTimeSmtpJobStepPasswordViewModel : SmtpJobStepPasswordViewModel
{
    public DesignTimeSmtpJobStepPasswordViewModel() : base(new TranslationUpdater(new TranslationFactory(), new ThreadManager()))
    {
        SmtpAccountInfo = "Hier könnte Ihr Smtp-Account stehen.";
    }
}
