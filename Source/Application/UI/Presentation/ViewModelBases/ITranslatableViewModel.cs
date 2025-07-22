using System.ComponentModel;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.ViewModelBases;

public interface ITranslatableViewModel<T> : INotifyPropertyChanged where T : ITranslatable
{
    T Translation { get; set; }
}
