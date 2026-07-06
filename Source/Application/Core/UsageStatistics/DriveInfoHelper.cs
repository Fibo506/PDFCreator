using System.IO;

namespace pdfforge.PDFCreator.Core.UsageStatistics;

public interface IDriveInfoHelper
{
    bool IsNetworkDrive(string path);
}

public class DriveInfoHelper(IDriveInfoWrapper driveInfoWrapper) : IDriveInfoHelper
{
    public bool IsNetworkDrive(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;
        if (path.TrimStart().StartsWith(@"\\"))
            return true;


        return driveInfoWrapper.DriveType(path) == DriveType.Network;
    }
}
