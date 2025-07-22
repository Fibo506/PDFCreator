using System.Diagnostics;
using System.Linq;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.UserControls.Accounts.AccountViews;

namespace pdfforge.PDFCreator.UI.Presentation.Commands.QuickActions;

public class QuickActionOpenExplorerLocationCommand : QuickActionCommandBase<FtpActionTranslation>
{
    public QuickActionOpenExplorerLocationCommand(ITranslationUpdater translationUpdater) : base(translationUpdater)
    {
    }

    public override void Execute(object obj)
    {
        var path = GetPaths(obj).FirstOrDefault();
        string args = $"/e, /select, \"{path}\"";
        Process.Start("explorer", args);
    }
}
