using System.Windows.Input;
using pdfforge.PDFCreator.Core.Services.Macros;

namespace pdfforge.PDFCreator.Core.Services;

public interface ICommandLocator
{
    ICommand GetCommand<T>() where T : class, ICommand;

    ICommand GetInitializedCommand<TCommand, TParameter>(TParameter parameter) where TCommand : class, IInitializedCommand<TParameter>;

    IMacroCommandBuilder CreateMacroCommand();
}
