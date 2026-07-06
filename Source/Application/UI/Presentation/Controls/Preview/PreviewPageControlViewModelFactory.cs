using pdfforge.Obsidian;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;

public interface IPreviewPageControlViewModelFactory
{
    PreviewPageControlViewModel Create(PreviewPage previewPage, bool isFreeEdition);
}

public class PreviewPageControlViewModelFactory : IPreviewPageControlViewModelFactory
{
    private readonly ITranslationUpdater _translationUpdater;
    private readonly IInteractionInvoker _interactionInvoker;

    public PreviewPageControlViewModelFactory(ITranslationUpdater translationUpdater, IInteractionInvoker interactionInvoker)
    {
        _translationUpdater = translationUpdater;
        _interactionInvoker = interactionInvoker;
    }

    public PreviewPageControlViewModel Create(PreviewPage previewPage, bool isFreeEdition)
    {
        return new PreviewPageControlViewModel(_translationUpdater, _interactionInvoker)
        {
            PreviewPage = previewPage,
            IsFreeEdition = isFreeEdition
        };
    }
}
