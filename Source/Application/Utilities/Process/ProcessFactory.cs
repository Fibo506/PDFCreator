using System.Diagnostics;

namespace pdfforge.PDFCreator.Utilities.Process;

public class ProcessWrapperFactory
{
    public virtual ProcessWrapper BuildProcessWrapper(ProcessStartInfo startInfo)
    {
        return new ProcessWrapper(startInfo);
    }
}
