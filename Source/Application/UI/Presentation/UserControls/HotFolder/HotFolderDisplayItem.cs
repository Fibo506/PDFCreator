using System.ComponentModel;
using System.Runtime.CompilerServices;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Core.Services.Translation;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
public class HotFolderDisplayItem : INotifyPropertyChanged
{
    private readonly IHotFolderConfigChecker _hotFolderConfigChecker;
    private readonly ErrorCodeInterpreter _errorCodeInterpreter;

    public HotFolderDisplayItem(IHotFolderConfigChecker hotFolderConfigChecker, ErrorCodeInterpreter errorCodeInterpreter)
    {
        _hotFolderConfigChecker = hotFolderConfigChecker;
        _errorCodeInterpreter = errorCodeInterpreter;
    }

    public HotFolderConfig HotFolderConfig { get; set; }
    public PrinterMappingWrapper PrinterMapping { get; set; }

    public string Printer => HotFolderConfig?.Printer;
    public string HotFolderPath => string.IsNullOrEmpty(HotFolderConfig?.HotFolderPath) ? ErrorMessage : HotFolderConfig?.HotFolderPath;

    public bool IsActive
    {
        get
        {
            if (HotFolderConfig == null || HasError)
                return false;

            return HotFolderConfig.IsActive;
        }
        set
        {
            if (HotFolderConfig != null)
            {
                if (HasError)
                    HotFolderConfig.IsActive = false;

                HotFolderConfig.IsActive = value;
                OnPropertyChanged();
            }
        }
    }
    public string ProfileName => PrinterMapping?.Profile?.Name ?? ProfileGuids.DEFAULT_PROFILE_GUID;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public string ErrorMessage
    {
        get
        {
            if (_hotFolderConfigChecker == null || HotFolderConfig == null)
                return string.Empty;

            var checkResult = _hotFolderConfigChecker.CheckForEditingConfig(HotFolderConfig, PrinterMapping.Profile.ConversionProfile);

            if (!checkResult)
                return _errorCodeInterpreter.GetFirstErrorText(checkResult, false);

            return string.Empty;
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == nameof(HotFolderConfig))
        {
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
    public void RefreshValidation()
    {
        OnPropertyChanged(nameof(HasError));
        OnPropertyChanged(nameof(ErrorMessage));
    }
}

