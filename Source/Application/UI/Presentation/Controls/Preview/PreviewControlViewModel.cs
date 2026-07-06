using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.Workflow;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.Controls;

public class PreviewControlViewModel : INotifyPropertyChanged
{
    private readonly IPreviewPageControlViewModelFactory _previewPageControlViewModelFactory;
    private ObservableCollection<PreviewPageControlViewModel> _previewPageViewModels;
    private readonly EditionHelper _editionHelper;
    private IPreviewManager _previewManager;
    private JobInfo _jobInfo;
    private bool _isPreviewLoading = true;
    private string _firstPageImagePath;
    private bool _isFreeEdition;
    private bool _expandPreview;

    public PreviewControlViewModel(IPreviewPageControlViewModelFactory previewPageControlViewModelFactory, EditionHelper editionHelper, IPreviewManager previewManager)
    {
        _previewPageControlViewModelFactory = previewPageControlViewModelFactory;
        _editionHelper = editionHelper;
        _previewManager = previewManager;
        _previewPageViewModels = [];
    }

    public ObservableCollection<PreviewPageControlViewModel> PreviewPageViewModels
    {
        get => _previewPageViewModels;
        set
        {
            _previewPageViewModels = value;
            OnPropertyChanged();
        }
    }

    public string FirstPageImagePath
    {
        get => _firstPageImagePath;
        private set
        {
            _firstPageImagePath = value;
            OnPropertyChanged();
        }
    }

    public JobInfo JobInfo
    {
        get => _jobInfo;
        set
        {
            _jobInfo = value;
            OnPropertyChanged();
            if (value != null)
            {
                _ = LoadPreviewPagesAsync();
            }
        }
    }

    public bool ExpandPreview
    {
        get => _expandPreview;
        set
        {
            _expandPreview = value;
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

    public bool IsFreeEdition
    {
        get => _isFreeEdition;
        set
        {
            _isFreeEdition = value;
            OnPropertyChanged();
        }
    }

    private async Task LoadPreviewPagesAsync()
    {
        if (JobInfo == null || _previewManager == null)
            return;

        PreviewPageViewModels.Clear();
        FirstPageImagePath = null;
        IsPreviewLoading = true;

        try
        {
            var previewPages = (await _previewManager.GetTotalPreviewPages(JobInfo)).ToList();

            if (previewPages.Count > 0)
            {
                FirstPageImagePath = await previewPages[0].PreviewImagePathTask;
            }

            foreach (var previewPage in previewPages)
            {
                var viewModel = _previewPageControlViewModelFactory.Create(previewPage, _editionHelper.IsFreeEdition);
                PreviewPageViewModels.Add(viewModel);
            }
        }

        finally
        {
            IsPreviewLoading = false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
