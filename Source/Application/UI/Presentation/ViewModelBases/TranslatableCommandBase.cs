using System;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.ViewModelBases;

public abstract class TranslatableCommandBase<TTranslatable> : ICommand where TTranslatable : ITranslatable, new()
{
    protected TTranslatable Translation;

    public TranslatableCommandBase(ITranslationUpdater translationUpdater)
    {
        translationUpdater.RegisterAndSetTranslation(SetTranslationAction);
    }

    private void SetTranslationAction(ITranslationFactory tf)
    {
        Translation = tf.UpdateOrCreateTranslation(Translation);
    }

    public abstract bool CanExecute(object parameter);

    public abstract void Execute(object parameter);

#pragma warning disable CS0067

    public event EventHandler CanExecuteChanged;

#pragma warning restore CS0067

    protected void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
