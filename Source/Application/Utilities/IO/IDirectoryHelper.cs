namespace pdfforge.PDFCreator.Utilities.IO;

public interface IDirectoryHelper
{
    bool CreateDirectory(string directory);

    bool DeleteCreatedDirectories();
}
