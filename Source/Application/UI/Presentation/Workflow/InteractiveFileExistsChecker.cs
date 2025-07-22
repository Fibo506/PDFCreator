using System.Globalization;
using System.Threading.Tasks;
using pdfforge.Obsidian.Trigger;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.UI.Interactions;
using pdfforge.PDFCreator.Utilities.Messages;
using pdfforge.PDFCreator.Utilities.Tokens;
using SystemInterface.IO;
using Translatable;

namespace pdfforge.PDFCreator.UI.Presentation.Workflow;

public interface IInteractiveFileExistsChecker
{
    Task<FileExistCheckResult> CheckIfFileExistsWithResultInOverlay(Job job, string latestConfirmedPath);
}

public class InteractiveFileExistsChecker : IInteractiveFileExistsChecker
{
    private readonly ITranslationFactory _translationFactory;
    private InteractiveProfileCheckerTranslation _translation;
    private readonly IFile _file;
    private readonly IInteractionRequest _interactionRequest;
    private readonly IFileIndexHelper _fileIndexHelper;

    public InteractiveFileExistsChecker(
        ITranslationFactory translationFactory,
        IFile file,
        IInteractionRequest interactionRequest,
        IFileIndexHelper fileIndexHelper)
    {
        _translationFactory = translationFactory;
        _file = file;
        _interactionRequest = interactionRequest;
        _fileIndexHelper = fileIndexHelper;
    }

    public async Task<FileExistCheckResult> CheckIfFileExistsWithResultInOverlay(Job job, string latestConfirmedPath)
    {
        var filePath = _fileIndexHelper.ReplaceFileIndex(job.OutputFileTemplate, job.TempOutputFiles.Count);

        //Do not inform user, if SaveFileDialog already did
        if (filePath == latestConfirmedPath)
            return new FileExistCheckResult(true, latestConfirmedPath);

        if (job.Profile.SaveFileTemporary || !_file.Exists(filePath))
            return new FileExistCheckResult(true, latestConfirmedPath);

        _translation = _translationFactory.UpdateOrCreateTranslation(_translation);
        var title = _translation.ConfirmSaveAs.ToUpper(CultureInfo.CurrentCulture);
        var text = _translation.GetFileAlreadyExists(job.OutputFileTemplate);

        var interaction = new MessageInteraction(text, title, MessageOptions.YesNo, MessageIcon.Exclamation);

        var result = await _interactionRequest.RaiseAsync(interaction);

        if (result.Response == MessageResponse.Yes)
            return new FileExistCheckResult(true, filePath);

        return new FileExistCheckResult(false, "");
    }
}

public class FileExistCheckResult
{
    public bool Success { get; }
    public string ConfirmedPath { get; }

    public FileExistCheckResult(bool success, string confirmedPath)
    {
        Success = success;
        ConfirmedPath = confirmedPath;
    }
}
