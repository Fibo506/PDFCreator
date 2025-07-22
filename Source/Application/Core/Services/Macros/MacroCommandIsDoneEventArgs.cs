using System;

namespace pdfforge.PDFCreator.Core.Services.Macros;

public class MacroCommandIsDoneEventArgs : EventArgs
{
    public ResponseStatus ResponseStatus { get; }

    public MacroCommandIsDoneEventArgs(ResponseStatus responseStatus)
    {
        ResponseStatus = responseStatus;
    }
}
