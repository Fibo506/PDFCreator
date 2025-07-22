using System.Windows.Input;

namespace pdfforge.PDFCreator.Core.Services;

public interface IInitializedCommand<T> : ICommand
{
    void Init(T parameter);
}
