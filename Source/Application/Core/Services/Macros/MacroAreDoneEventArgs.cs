using System;

namespace pdfforge.PDFCreator.Core.Services.Macros;

public class MacroAreDoneEventArgs : EventArgs
{
    public IMacroResult Result { get; }

    public MacroAreDoneEventArgs(IMacroResult result)
    {
        Result = result;
    }
}
