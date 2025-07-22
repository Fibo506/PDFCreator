using System.Threading.Tasks;
using pdfforge.PDFCreator.Core.Services.Update;
using pdfforge.PDFCreator.UI.Presentation.Assistants.Update;

namespace pdfforge.PDFCreator.UI.Presentation.DesignTime.Helper;

public class DesignTimeUpdateLauncher : IUpdateLauncher
{
    public Task LaunchUpdateAsync(IApplicationVersion version)
    {
        return Task.FromResult((object)null);
    }
}
