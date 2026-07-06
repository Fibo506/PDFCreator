using pdfforge.Obsidian.Interaction;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;

public class AddHotFolderFilterUserInteraction : IInteraction
{
    public string FileExtension { get; set; }

    public AddHotFolderFilterUserInteraction(string fileExtension)
    {
        FileExtension = fileExtension;
    }
    public bool Success { get; set; }

}
