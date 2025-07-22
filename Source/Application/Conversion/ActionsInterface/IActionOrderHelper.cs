using System.Collections.Generic;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.Conversion.ActionsInterface;

public interface IActionOrderHelper
{
    void EnsureValidOrder(List<string> currentActionOrderList);

    void CleanUpAndEnsureValidOrder(IEnumerable<ConversionProfile> profiles);
}
