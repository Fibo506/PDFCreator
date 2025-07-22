using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using Prism.Mvvm;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.ViewModelBases;

public abstract class TranslatableViewModelBase<T> : BindableBase, ITranslatableViewModel<T> where T : ITranslatable, new()
{
    private T _translation;

    public T Translation
    {
        get { return _translation; }
        set
        {
            _translation = value;
            RaisePropertyChanged(nameof(Translation));
            OnTranslationChanged();
        }
    }

    protected virtual void OnTranslationChanged()
    {
    }

    protected TranslatableViewModelBase(ITranslationUpdater translationUpdater)
    {
        translationUpdater.RegisterAndSetTranslation(this);
    }
}
