using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Core.SettingsManagement;

namespace pdfforge.PDFCreator.UI.COM;
public class ComDefaultSettingsBuilder(IActionOrderHelper actionOrderHelper) : PDFCreatorDefaultSettingsBuilder(actionOrderHelper)
{
    public override ConversionProfile CreateDefaultProfile()
    {
        var conversionProfile = base.CreateDefaultProfile();
        conversionProfile.OpenViewer.Enabled = false;
        conversionProfile.Guid = ProfileGuids.DEFAULT_PROFILE_COM_GUID;
        conversionProfile.Name = "DefaultForCom";
        conversionProfile.AutoSave.ExistingFileBehaviour = AutoSaveExistingFileBehaviour.Overwrite;
        return conversionProfile;
    }
}
