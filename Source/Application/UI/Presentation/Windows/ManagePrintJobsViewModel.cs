using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using NaturalSort.Extension;
using pdfforge.Obsidian;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.JobInfoQueue;
using pdfforge.PDFCreator.Core.Services;
using pdfforge.PDFCreator.UI.Interactions;
using pdfforge.PDFCreator.UI.Presentation.Commands;
using pdfforge.PDFCreator.UI.Presentation.Controls;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using pdfforge.PDFCreator.UI.Presentation.Helper.Translation;
using pdfforge.PDFCreator.UI.Presentation.ViewModelBases;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.UI.Presentation.Windows;

public class ManagePrintJobsViewModel : OverlayViewModelBase<ManagePrintJobsInteraction, ManagePrintJobsWindowTranslation>
{
    private readonly DragAndDropEventHandler _dragAndDrop;
    private readonly IJobInfoManager _jobInfoManager;
    private readonly IDispatcher _dispatcher;
    private readonly ApplicationNameProvider _applicationNameProvider;
    private readonly IVersionHelper _versionHelper;
    private readonly IJobInfoQueue _jobInfoQueue;
    private readonly IPreviewControlViewModelFactory _previewControlViewModelFactory;
    private readonly ObservableCollection<JobInfoPreviewWrapper> _jobInfoPreviewWrappers;
    private Helper.SynchronizedCollection<JobInfoPreviewWrapper> _synchronizedJobs;

    public IDropTarget CustomDropHandler { get; } = new CustomDropEventHandler();

    public ManagePrintJobsViewModel(IJobInfoQueue jobInfoQueue, DragAndDropEventHandler dragAndDrop, IJobInfoManager jobInfoManager,
        IDispatcher dispatcher, ITranslationUpdater translationUpdater, ApplicationNameProvider applicationNameProvider,
        IVersionHelper versionHelper, ICommandLocator commandLocator, IPreviewControlViewModelFactory previewControlViewModelFactory)
        : base(translationUpdater)
    {
        _jobInfoQueue = jobInfoQueue;
        _dragAndDrop = dragAndDrop;
        _jobInfoManager = jobInfoManager;
        _dispatcher = dispatcher;
        _applicationNameProvider = applicationNameProvider;
        _versionHelper = versionHelper;
        _jobInfoQueue.OnNewJobInfo += OnNewJobInfo;
        _previewControlViewModelFactory = previewControlViewModelFactory;

        ConvertFileCommand = commandLocator.GetCommand<SelectFileViaDialogAndConvertCommand>();
        DeleteJobCommand = new DelegateCommand(DeleteJobExecute);
        MergeJobsCommand = new DelegateCommand(ExecuteMergeJobs, CanExecuteMergeJobs);
        MergeAllJobsCommand = new DelegateCommand(ExecuteMergeAllJobs, HasMoreThanOneJob);
        SelectUnSelectAllJobsCommand = new DelegateCommand(SelectUnSelectAllJobsExecute);
        WindowClosedCommand = new DelegateCommand(OnWindowClosed);
        WindowActivatedCommand = new DelegateCommand(OnWindowActivated);
        DragEnterCommand = new DelegateCommand<DragEventArgs>(OnDragEnter);
        DropCommand = new DelegateCommand<DragEventArgs>(OnDrop);
        KeyDownCommand = new DelegateCommand<KeyEventArgs>(OnKeyDown);

        SortCommand = new DelegateCommand(SortCommandExecute, HasMoreThanOneJob);
        SetupSortMenuItems();

        _synchronizedJobs = new Helper.SynchronizedCollection<JobInfoPreviewWrapper>(
            new ObservableCollection<JobInfoPreviewWrapper>(
                _jobInfoQueue.JobInfos.Select(CreateJobInfoPreviewWrapper)
            )
        );
        _jobInfoPreviewWrappers = _synchronizedJobs.ObservableCollection;
        JobInfos = new CollectionView(_jobInfoPreviewWrappers);
        JobListSelectionChangedCommand = new DelegateCommand(JobListSelectionChangedExecute);
    }

    private void SetupSortMenuItems()
    {
        SortMenuItems = new List<MenuItem>
        {
            new MenuItem { Header = Translation.IdAscending, CommandParameter = MergeSortingEnum.IdAscending },
            new MenuItem { Header = Translation.IdDescending, CommandParameter = MergeSortingEnum.IdDescending },
            new MenuItem { Header = Translation.NameAscending, CommandParameter = MergeSortingEnum.NameAscending },
            new MenuItem { Header = Translation.NameDescending, CommandParameter = MergeSortingEnum.NameDescending },
            new MenuItem { Header = Translation.DateAscending, CommandParameter = MergeSortingEnum.DateAscending },
            new MenuItem { Header = Translation.DateDescending, CommandParameter = MergeSortingEnum.DateDescending }
        };
    }

    private void SortCommandExecute(object parameter)
    {
        var list = _jobInfoPreviewWrappers.ToList();

        switch ((MergeSortingEnum)parameter)
        {
            case MergeSortingEnum.IdAscending:
                list = list.OrderBy(info => info.JobInfo.SourceFiles[0].JobCounter).ToList();
                break;

            case MergeSortingEnum.IdDescending:
                list = list.OrderByDescending(info => info.JobInfo.SourceFiles[0].JobCounter).ToList();
                break;

            case MergeSortingEnum.NameAscending:
                list = list.OrderBy(info => info.JobInfo.Metadata.PrintJobName, StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();
                break;

            case MergeSortingEnum.NameDescending:
                list = list.OrderByDescending(info => info.JobInfo.Metadata.PrintJobName, StringComparison.OrdinalIgnoreCase.WithNaturalSort()).ToList();
                break;

            case MergeSortingEnum.DateAscending:
                list = list.OrderBy(info => info.JobInfo.PrintDateTime).ToList();
                break;

            case MergeSortingEnum.DateDescending:
                list = list.OrderByDescending(info => info.JobInfo.PrintDateTime).ToList();
                break;
        }

        _jobInfoPreviewWrappers.Clear();

        foreach (var jobInfo in list)
        {
            _jobInfoPreviewWrappers.Add(jobInfo);
        }

        JobInfos.Refresh();
    }

    private void JobListSelectionChangedExecute(object obj)
    {
        MergeJobsCommand.RaiseCanExecuteChanged();
        MergeAllJobsCommand.RaiseCanExecuteChanged();

        if (!_suppressUncheckOfSelectUnselectAllOnSelectedChanged)
        {
            SelectUnselectAll = false;
            RaisePropertyChanged(nameof(SelectUnselectAll));
        }
    }

    public CollectionView JobInfos { get; private set; }
    public ICommand ConvertFileCommand { get; set; }
    public DelegateCommand DeleteJobCommand { get; }
    public DelegateCommand MergeJobsCommand { get; }
    public DelegateCommand MergeAllJobsCommand { get; }
    public DelegateCommand SelectUnSelectAllJobsCommand { get; }
    public DelegateCommand WindowClosedCommand { get; }
    public DelegateCommand WindowActivatedCommand { get; }

    public IEnumerable<MenuItem> SortMenuItems { get; private set; }

    public DelegateCommand SortCommand { get; }
    public DelegateCommand<DragEventArgs> DragEnterCommand { get; }
    public DelegateCommand<DragEventArgs> DropCommand { get; }
    public DelegateCommand<KeyEventArgs> KeyDownCommand { get; }

    public DelegateCommand JobListSelectionChangedCommand { get; set; }

    private void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            FinishInteraction();
    }

    private void OnWindowActivated(object obj)
    {
        MergeJobsCommand.RaiseCanExecuteChanged();
        MergeAllJobsCommand.RaiseCanExecuteChanged();
        SortCommand.RaiseCanExecuteChanged();
    }

    private void OnDrop(DragEventArgs e)
    {
        _dragAndDrop.HandleDropEvent(e);
    }

    private void OnDragEnter(DragEventArgs e)
    {
        _dragAndDrop.HandleDragEnter(e);
    }

    private void OnWindowClosed(object obj)
    {
        _jobInfoQueue.OnNewJobInfo -= OnNewJobInfo;
    }

    private void OnNewJobInfo(object sender, NewJobInfoEventArgs e)
    {
        Action<JobInfo> addMethod = AddJobInfo;
        _dispatcher.BeginInvoke(addMethod, e.JobInfo);
    }

    // Commenting out as part of PC-5615

    //public async Task MergePreviewDragDrop(IEnumerable<string> files)
    //{
    //    try
    //    {
    //        // Prevent UI addition of the JobInfo that was added to the preview,
    //        // ensures that it only updates after the full merge is done.
    //        _jobInfoQueue.OnNewJobInfo -= OnNewJobInfo;
    //        var jobInfoToMerge = await _fileConversionAssistant.GetJobInfoForPreviewMerge(files);

    //        if (jobInfoToMerge == null)
    //            return;

    //        var targetJob = _jobInfoQueue.JobInfos.ToList().First();
    //        await MergeWithCurrentJobInfo(targetJob, jobInfoToMerge);
    //    }
    //    catch (Exception ex)
    //    {
    //        // TODO: Handle exceptions that may occur during file merging (design / content wise)
    //        MessageBox.Show($"Error merging files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //    }
    //    finally
    //    {
    //        _jobInfoQueue.OnNewJobInfo += OnNewJobInfo;
    //    }
    //}

    //private Task MergeWithCurrentJobInfo(JobInfo targetJob, JobInfo newJob)
    //{
    //    if (newJob != null && newJob.JobType == targetJob.JobType)
    //    {
    //        _synchronizedJobs.SuspendUpdates();
    //        try
    //        {
    //            _jobInfoManager.Merge(targetJob, newJob);

    //            if (_jobInfos.Contains(newJob))
    //            {
    //                _jobInfos.Remove(newJob);
    //            }

    //            _jobInfoQueue.Remove(newJob, false);
    //            _jobInfoManager.DeleteInf(newJob);
    //            _jobInfoManager.SaveToInfFile(targetJob);
    //        }
    //        finally
    //        {
    //            _synchronizedJobs.ResumeUpdates();
    //        }

    //        MergeJobsCommand.RaiseCanExecuteChanged();
    //        MergeAllJobsCommand.RaiseCanExecuteChanged();
    //        SortCommand.RaiseCanExecuteChanged();

    //        JobInfos.Refresh();
    //    }
    //    return Task.CompletedTask;
    //}

    private void AddJobInfo(JobInfo jobInfo)
    {
        if (_jobInfoPreviewWrappers.Any(w => w.JobInfo == jobInfo))
            return;

        var wrapper = CreateJobInfoPreviewWrapper(jobInfo);

        var nextJob = _jobInfoPreviewWrappers.FirstOrDefault(w => w.JobInfo.PrintDateTime > jobInfo.PrintDateTime);

        var targetPosition = nextJob == null
            ? _jobInfoPreviewWrappers.Count
            : _jobInfoPreviewWrappers.IndexOf(nextJob);

        _jobInfoPreviewWrappers.Insert(targetPosition, wrapper);

        if (JobInfos.CurrentItem == null)
            JobInfos.MoveCurrentToFirst();

        MergeJobsCommand.RaiseCanExecuteChanged();
        MergeAllJobsCommand.RaiseCanExecuteChanged();
        SortCommand.RaiseCanExecuteChanged();

        SelectUnselectAll = false;
        RaisePropertyChanged(nameof(SelectUnselectAll));
    }

    private void DeleteJobExecute(object o)
    {
        //var position = JobInfos.CurrentPosition;

        if (o is not JobInfoPreviewWrapper wrapper)
            return;

        _jobInfoPreviewWrappers.Remove(wrapper);
        _jobInfoQueue.Remove(wrapper.JobInfo, true);

        //if (_jobInfos.Count > 0)
        //    JobInfos.MoveCurrentToPosition(Math.Max(0, position - 1));

        MergeJobsCommand.RaiseCanExecuteChanged();
        MergeAllJobsCommand.RaiseCanExecuteChanged();
        SortCommand.RaiseCanExecuteChanged();
    }

    private void ExecuteMergeJobs(object o)
    {
        if (!CanExecuteMergeJobs(o))
            throw new InvalidOperationException("CanExecute is false");

        var wrapperObjects = o as IEnumerable<object>;
        if (wrapperObjects == null)
            return;

        var wrappers = wrapperObjects.Cast<JobInfoPreviewWrapper>().ToList();
        var first = wrappers.First();

        foreach (var wrapper in wrappers.Skip(1))
        {
            if (wrapper.JobInfo.JobType != first.JobInfo.JobType)
                continue;

            _jobInfoManager.Merge(first.JobInfo, wrapper.JobInfo);
            _jobInfoPreviewWrappers.Remove(wrapper);
            _jobInfoQueue.Remove(wrapper.JobInfo, false);
            _jobInfoManager.DeleteInf(wrapper.JobInfo);
        }

        // Update the first wrapper's PreviewViewModel with the merged JobInfo
        first.PreviewViewModel.JobInfo = first.JobInfo;

        _jobInfoManager.SaveToInfFile(first.JobInfo);

        MergeJobsCommand.RaiseCanExecuteChanged();
        MergeAllJobsCommand.RaiseCanExecuteChanged();
        SortCommand.RaiseCanExecuteChanged();

        JobInfos.Refresh();
    }


    public bool SelectUnselectAll { get; set; } = false;

    private bool _suppressUncheckOfSelectUnselectAllOnSelectedChanged = false;

    private void SelectUnSelectAllJobsExecute(object o)
    {
        if (o is not ListBox listBox)
            return;

        _suppressUncheckOfSelectUnselectAllOnSelectedChanged = true;
        if (SelectUnselectAll)
            listBox.SelectAll();
        else
            listBox.UnselectAll();

        ResetLastSelectedItem?.Invoke();

        _suppressUncheckOfSelectUnselectAllOnSelectedChanged = false;
    }

    public Action ResetLastSelectedItem { get; set; }

    private bool CanExecuteMergeJobs(object o)
    {
        var jobs = o as IEnumerable<object>;
        return jobs != null && jobs.Count() > 1;
    }

    private void ExecuteMergeAllJobs(object o)
    {
        ExecuteMergeJobs(_jobInfoPreviewWrappers);
    }

    private bool HasMoreThanOneJob(object o)
    {
        return _jobInfoPreviewWrappers.Count > 1;
    }

    private JobInfoPreviewWrapper CreateJobInfoPreviewWrapper(JobInfo jobInfo)
    {
        var previewViewModel = _previewControlViewModelFactory.Create(jobInfo);

        return new JobInfoPreviewWrapper
        {
            PreviewViewModel = previewViewModel,
            JobInfo = jobInfo
        };
    }

    public override string Title => _applicationNameProvider.ApplicationNameWithEdition + " " + _versionHelper.FormatWithThreeDigits();
}

public enum MergeSortingEnum
{
    IdAscending,
    IdDescending,
    NameAscending,
    NameDescending,
    DateAscending,
    DateDescending
}

public class JobInfoPreviewWrapper : INotifyPropertyChanged
{
    private JobInfo _jobInfo;

    public JobInfo JobInfo
    {
        get => _jobInfo;
        set
        {
            _jobInfo = value;
            OnPropertyChanged();
        }
    }
    public PreviewControlViewModel PreviewViewModel { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
