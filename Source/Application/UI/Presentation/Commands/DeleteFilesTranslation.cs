using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.Commands;

public class DeleteFilesTranslation : ITranslatable
{
    private IPluralBuilder PluralBuilder { get; set; } = new DefaultPluralBuilder();

    private string[] AndXMore { get; set; } = { "and {0} more.", "and {0} more." };

    public string GetAndXMoreMessage(int numberOfFiles)
    {
        return PluralBuilder.GetFormattedPlural(numberOfFiles, AndXMore);
    }

    private string[] DeleteFilesTitle { get; set; } = { "Delete File", "Delete Files" };

    public string GetDeleteFilesTitle(int numberOfFiles)
    {
        return PluralBuilder.GetFormattedPlural(numberOfFiles, DeleteFilesTitle);
    }

    private string[] AreYouSureYouWantToDeleteFiles { get; set; } = { "Are you sure you want to delete the following file?", "Are you sure you want to delete the following files?" };

    public string GetAreYouSureYouWantToDeleteFilesMessage(int numberOfFiles)
    {
        return PluralBuilder.GetFormattedPlural(numberOfFiles, AreYouSureYouWantToDeleteFiles);
    }

    public string ErrorDuringDeletionTitle { get; private set; } = "Error During Deletion";

    private string[] CouldNotDeleteTheFollowingFiles { get; set; } = { "Could not delete the following file.\nMaybe the file is currently in use.\nPlease try again later.", "Could not delete the following files.\nMaybe the files are currently in use.\nPlease try again later." };

    public string GetCouldNotDeleteTheFollowingFilesMessage(int numberOfFiles)
    {
        return PluralBuilder.GetFormattedPlural(numberOfFiles, CouldNotDeleteTheFollowingFiles);
    }
}
