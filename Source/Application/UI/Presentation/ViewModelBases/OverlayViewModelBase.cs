using pdfforge.Obsidian.Interaction;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.ViewModelBases;

public abstract class OverlayViewModelBase<TInteraction, TTranslatable>
    : InteractionAwareViewModelBase<TInteraction>, ITranslatableViewModel<TTranslatable>
    where TTranslatable : ITranslatable, new()
    where TInteraction : class, IInteraction
{
    private TTranslatable _translation;

    public TTranslatable Translation
    {
        get { return _translation; }
        set
        {
            _translation = value;
            RaisePropertyChanged(nameof(Translation));
            OnTranslationChanged();
        }
    }

    protected OverlayViewModelBase(ITranslationUpdater translationUpdater)
    {
        translationUpdater.RegisterAndSetTranslation(this);
    }

    protected virtual void OnTranslationChanged()
    {
    }
}
