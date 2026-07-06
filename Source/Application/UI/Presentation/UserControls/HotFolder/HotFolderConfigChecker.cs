using System;
using System.Collections.ObjectModel;
using System.Linq;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Settings;
using pdfforge.PDFCreator.Conversion.Settings.HotFolder.Enums;
using pdfforge.PDFCreator.UI.Presentation.Wrapper;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Tokens;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

public interface IHotFolderConfigChecker
{
    public ActionResult CheckForEditingConfig(HotFolderConfig hotFolderConfig, ConversionProfile profile);
    public ActionResult CheckForStartingHotFolder(HotFolderConfig hotFolderConfig);
    public bool IsCurrentProfileOutputPathSameAsHotFolderPath(ConversionProfile currentProfile, ObservableCollection<PrinterMappingWrapper> printerMappingWrappers);
}

public class HotFolderConfigChecker : IHotFolderConfigChecker
{
    private readonly IPathUtil _pathUtil;
    private readonly ICurrentSettings<HotFolderSettings> _hotFolderSettings;

    public HotFolderConfigChecker(IPathUtil pathUtil, ICurrentSettings<HotFolderSettings> hotFolderSettings)
    {
        _pathUtil = pathUtil;
        _hotFolderSettings = hotFolderSettings;
    }

    public ActionResult CheckForEditingConfig(HotFolderConfig hotFolderConfig, ConversionProfile profile)
    {
        var result = CheckHotFolderConfig(hotFolderConfig, CheckLevel.EditingProfile);

        if (result && string.Equals(hotFolderConfig.HotFolderPath, profile.TargetDirectory, StringComparison.InvariantCultureIgnoreCase))
            return new ActionResult(ErrorCode.HotFolder_ProfileTargetDirAndHotFolderPathAreEqual);
        
        return result;
    }

    private ActionResult CheckHotFolderConfig(HotFolderConfig hotFolderConfig, CheckLevel checkLevel)
    {
        // Check HotFolder Path
        if (checkLevel == CheckLevel.RunningJob || !TokenIdentifier.ContainsTokens(hotFolderConfig.HotFolderPath))
        {
            var hotFolderPathStatus = _pathUtil.IsValidRootedPathWithResponse(hotFolderConfig.HotFolderPath);
            if (hotFolderPathStatus != PathUtilStatus.Success)
            {
                return hotFolderPathStatus switch
                {
                    PathUtilStatus.InvalidPath => new ActionResult(ErrorCode.HotFolder_PathInvalid),
                    PathUtilStatus.PathWasNullOrEmpty => new ActionResult(ErrorCode.HotFolder_PathEmpty),
                    PathUtilStatus.PathTooLongEx => new ActionResult(ErrorCode.HotFolder_PathTooLong),
                    _ => new ActionResult()
                };
            }
        }

        // Check SourceFile Path (if move to location is selected)
        if (hotFolderConfig.SourceFileMover == FileMover.MoveToLocation)
        {
            if (checkLevel == CheckLevel.RunningJob || !TokenIdentifier.ContainsTokens(hotFolderConfig.SourceFilesPath))
            {
                var sourceFileMoverPathStatus = _pathUtil.IsValidRootedPathWithResponse(hotFolderConfig.SourceFilesPath);
                if (sourceFileMoverPathStatus != PathUtilStatus.Success)
                {
                    return sourceFileMoverPathStatus switch
                    {
                        PathUtilStatus.InvalidPath => new ActionResult(ErrorCode.HotFolder_SourceFileMoverPathInvalid),
                        PathUtilStatus.PathWasNullOrEmpty => new ActionResult(ErrorCode.HotFolder_SourceFileMoverPathEmpty),
                        PathUtilStatus.PathTooLongEx => new ActionResult(ErrorCode.HotFolder_SourceFileMoverPathTooLong),
                        _ => new ActionResult()
                    };
                }
            }

            if (string.Equals(hotFolderConfig.SourceFilesPath, hotFolderConfig.HotFolderPath, StringComparison.InvariantCultureIgnoreCase))
                return new ActionResult(ErrorCode.HotFolder_SourceAndHotFolderPathAreEqual);

            if (_pathUtil.IsSubdirectory(hotFolderConfig.SourceFilesPath, hotFolderConfig.HotFolderPath))
                return new ActionResult(ErrorCode.HotFolder_SourcePathIsSubfolderOfHotFolder);
        }

        // Check UnprintableFile Path (if move to location is selected)
        if (hotFolderConfig.UnprintableFileMover == FileMover.MoveToLocation)
        {
            if (checkLevel == CheckLevel.RunningJob || !TokenIdentifier.ContainsTokens(hotFolderConfig.UnprintableFilesPath))
            {
                var unprintableFileMoverPathStatus = _pathUtil.IsValidRootedPathWithResponse(hotFolderConfig.UnprintableFilesPath);
                if (unprintableFileMoverPathStatus != PathUtilStatus.Success)
                {
                    return unprintableFileMoverPathStatus switch
                    {
                        PathUtilStatus.InvalidPath => new ActionResult(ErrorCode.HotFolder_UnprintableFileMoverPathInvalid),
                        PathUtilStatus.PathWasNullOrEmpty => new ActionResult(ErrorCode.HotFolder_UnprintableFileMoverPathEmpty),
                        PathUtilStatus.PathTooLongEx => new ActionResult(ErrorCode.HotFolder_UnprintableFileMoverPathTooLong),
                        _ => new ActionResult()
                    };
                }
            }

            if (string.Equals(hotFolderConfig.UnprintableFilesPath, hotFolderConfig.HotFolderPath, StringComparison.InvariantCultureIgnoreCase))
                return new ActionResult(ErrorCode.HotFolder_UnprintablePathAndHotFolderPathAreEqual);

            if (_pathUtil.IsSubdirectory(hotFolderConfig.UnprintableFilesPath, hotFolderConfig.HotFolderPath))
                return new ActionResult(ErrorCode.HotFolder_UnprintablePathIsSubFolderOfHotFolder);
        }

        return new ActionResult();
    }

    public ActionResult CheckForStartingHotFolder(HotFolderConfig hotFolderConfig)
    {
        return CheckHotFolderConfig(hotFolderConfig, CheckLevel.RunningJob);
    }

    public bool IsCurrentProfileOutputPathSameAsHotFolderPath(ConversionProfile currentProfile, ObservableCollection<PrinterMappingWrapper> printerMappingWrappers)
    {
        var hotFolderPrinters = printerMappingWrappers.Where(pm => pm.IsHotFolder);

        var sameProfileHotFolderPrinters = hotFolderPrinters.
            Where(pm => pm.Profile.ConversionProfile.Guid == currentProfile.Guid);

        var sameProfileHotFolderPrinterNames = sameProfileHotFolderPrinters
            .Select(pm => pm.PrinterName)
            .ToList();

        var sameProfileHotFolderConfigs = _hotFolderSettings.Settings.HotFolderConfigs
            .Where(hfc => sameProfileHotFolderPrinterNames.Contains(hfc.Printer));

        return sameProfileHotFolderConfigs.Any(hfc => _pathUtil.IsSubdirectory(currentProfile.TargetDirectory, hfc.HotFolderPath));
    }
}

public class EmptyHotFolderConfigChecker : IHotFolderConfigChecker
{
    public ActionResult CheckForEditingConfig(HotFolderConfig hotFolderConfig, ConversionProfile profile)
    {
        return new ActionResult();
    }

    public ActionResult CheckForStartingHotFolder(HotFolderConfig hotFolderConfig)
    {
        return new ActionResult();
    }

    public bool IsCurrentProfileOutputPathSameAsHotFolderPath(ConversionProfile currentProfile, ObservableCollection<PrinterMappingWrapper> printerMappingWrappers)
    {
        return false;
    }
}
