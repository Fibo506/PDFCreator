using System.Collections.Generic;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Conversion.ActionsInterface;

public interface IActionManager
{
    IEnumerable<T> GetEnabledActionsInCurrentOrder<T>(ConversionProfile profile) where T : IAction;

    IEnumerable<T> GetEnabledActionsInCurrentOrder<T>(Job job) where T : IAction;

    bool HasSendActions(ConversionProfile profile);
}
