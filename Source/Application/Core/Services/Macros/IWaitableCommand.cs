using System;
using System.Windows.Input;

namespace pdfforge.PDFCreator.Core.Services.Macros;

public interface IWaitableCommand : ICommand
{
    event EventHandler<MacroCommandIsDoneEventArgs> IsDone;
}
