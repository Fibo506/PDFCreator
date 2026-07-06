using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.UI.Presentation.Windows.ProfessionalFeatureInteractions;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;
public class PreviewPageControlViewModel : TranslatableViewModelBase<PreviewPageControlViewTranslation>, INotifyPropertyChanged
{
    private PreviewPage _previewPage;
    private bool _isPreviewLoading = true;
    private string _previewImagePath = "";
    private double _currentRotation;
    private bool _removePage;
    private bool _isFreeEdition;
    private IInteractionInvoker _interactionInvoker;

    public event PropertyChangedEventHandler PropertyChanged;

    public PreviewPageControlViewModel(ITranslationUpdater translationUpdater, IInteractionInvoker interactionInvoker) : base(translationUpdater)
    {
        _interactionInvoker = interactionInvoker;
    }

    public PreviewPage PreviewPage
    {
        get => _previewPage;
        set
        {
            if (_previewPage != null)
            {
                _previewPage.PropertyChanged -= PreviewPage_PropertyChanged;
            }

            _previewPage = value;
            OnPropertyChanged();

            if (_previewPage != null)
            {
                _previewPage.PropertyChanged += PreviewPage_PropertyChanged;
                _ = LoadPreviewImageAsync();
            }
        }
    }

    public double CurrentRotation
    {
        get => _currentRotation;
        set
        {
            _currentRotation = value;
            OnPropertyChanged();
        }
    }

    public bool RemovePage
    {
        get => _removePage;
        set
        {
            _removePage = value;
            OnPropertyChanged();
        }
    }

    public bool IsPreviewLoading
    {
        get => _isPreviewLoading;
        set
        {
            _isPreviewLoading = value;
            OnPropertyChanged();
        }
    }

    public string PreviewImagePath
    {
        get => _previewImagePath;
        set
        {
            _previewImagePath = value;
            OnPropertyChanged();
        }
    }

    public bool IsFreeEdition
    {
        get => _isFreeEdition;
        set
        {
            _isFreeEdition = value;
            OnPropertyChanged();
        }
    }

    public ICommand RotatePageCommand => new DelegateCommand(o =>
    {
        if (PreviewPage != null)
        {
            PreviewPage.RotationAngle = (PreviewPage.RotationAngle + 90) % 360;
            CurrentRotation = PreviewPage.RotationAngle;
        }
    });

    public ICommand RemovePageCommand => new DelegateCommand(o =>
    {
        if (PreviewPage != null)
        {
            if (!IsFreeEdition)
            {
                RemovePage = !RemovePage;
                PreviewPage.IsExcluded = RemovePage;
            }
            else
            {
                var interaction = new BusinessFeaturesUserInteraction();
                _interactionInvoker.Invoke(interaction);
            }
        }
    });

    public string DeleteButtonToolTipText => IsFreeEdition ? Translation.BusinessFeature : Translation.RemovePage;

    private async Task LoadPreviewImageAsync()
    {
        if (PreviewPage == null)
            return;

        IsPreviewLoading = true;
        PreviewImagePath = await PreviewPage.PreviewImagePathTask.ConfigureAwait(false);
        CurrentRotation = PreviewPage.RotationAngle;
        RemovePage = PreviewPage.IsExcluded;
        IsPreviewLoading = false;
    }

    private void PreviewPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PreviewPage.RotationAngle))
        {
            CurrentRotation = PreviewPage.RotationAngle;
        }
        if (e.PropertyName == nameof(PreviewPage.IsExcluded))
        {
            RemovePage = PreviewPage.IsExcluded;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
