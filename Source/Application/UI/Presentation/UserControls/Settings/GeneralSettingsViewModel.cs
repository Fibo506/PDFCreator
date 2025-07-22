using pdfforge.PDFCreator.Core.Services;
using Prism.Mvvm;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.Settings;

public class GeneralSettingsViewModel : BindableBase, IMountable
{
    public GeneralSettingsViewModel()
    {
    }

    public void MountView()
    {
    }

    public void UnmountView()
    {
    }
}

public class DesignTimeGeneralSettingsViewModel : GeneralSettingsViewModel
{
    public DesignTimeGeneralSettingsViewModel() : base()
    {
    }
}
