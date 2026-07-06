using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace pdfforge.PDFCreator.Core.Workflow;

public class PreviewPage : INotifyPropertyChanged
{
    public int PageNumber { get; set; }
    public Task<string> PreviewImagePathTask { get; set; }
    public int SourcePageNumber { get; set; }

    private int _rotationAngle;
    public int RotationAngle
    {
        get => _rotationAngle;
        set
        {
            if (_rotationAngle != value)
            {
                _rotationAngle = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isExcluded;
    public bool IsExcluded
    {
        get => _isExcluded;
        set
        {
            if (_isExcluded != value)
            {
                _isExcluded = value;
                OnPropertyChanged();
            }
        }
    }

    public PreviewPage(int pageNumber, Task<string> previewImagePathTask)
    {
        PreviewImagePathTask = previewImagePathTask;
        PageNumber = pageNumber;
        RotationAngle = 0;
        IsExcluded = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class PreviewPages
{
    public string Directory { get; }
    public IList<PreviewPage> PreviewPageList { get; set; } = new List<PreviewPage>();
    public Action DisposeDocument { get; set; }

    public PreviewPages(string directory)
    {
        Directory = directory;
    }
}
