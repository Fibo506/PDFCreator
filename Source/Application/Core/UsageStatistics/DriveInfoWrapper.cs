using System.IO;

namespace pdfforge.PDFCreator.Core.UsageStatistics;

public interface IDriveInfoWrapper
{
    DriveType DriveType(string driveName);
}

public class DriveInfoWrapper : IDriveInfoWrapper
{
    public DriveType DriveType(string driveName)
    {
        try
        {
            return new DriveInfo(driveName).DriveType;
        }
        catch
        {
            return System.IO.DriveType.Unknown;
        }
    }
}
