using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Jobs.Query;
using pdfforge.PDFCreator.Conversion.Settings.Enums;
using pdfforge.PDFCreator.Core.Workflow.Queries;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.IO;
using pdfforge.PDFCreator.Utilities.Tokens;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Workflow.Output;

public class InteractiveOutputFileMover : OutputFileMoverBase
{
    private readonly IDispatcher _dispatcher;

    public InteractiveOutputFileMover(IUniqueFilenameFactory uniqueFilenameFactory, IFile file, IPathUtil pathUtil,
        IDirectoryHelper directoryHelper, IPdfProcessor pdfProcessor, IFileIndexHelper fileIndexHelper,
        IRetypeFileNameQuery retypeFileNameQuery, IDispatcher dispatcher)
    : base(uniqueFilenameFactory, file, pathUtil, directoryHelper, pdfProcessor, fileIndexHelper)
    {
        _dispatcher = dispatcher;
        RetypeFileNameQuery = retypeFileNameQuery;
    }

    private IRetypeFileNameQuery RetypeFileNameQuery { get; }

    protected override Task<QueryResult<string>> HandleInvalidRootedPath(string filename, OutputFormat outputFormat)
    {
        var result = _dispatcher.InvokeAsync(() => RetypeFileNameQuery.RetypeFileNameQuery(filename, outputFormat, RetypeReason.InvalidRootedPath));
        return result;
    }

    protected override Task<QueryResult<string>> HandleFirstFileFailed(string filename, OutputFormat outputFormat)
    {
        var result = _dispatcher.InvokeAsync(() => RetypeFileNameQuery.RetypeFileNameQuery(filename, outputFormat, RetypeReason.CopyError));
        return result;
    }

    protected override HandleCopyErrorResult QueryHandleCopyError(int fileNumber)
    {
        if (fileNumber == 1)
            return HandleCopyErrorResult.Requery;

        return HandleCopyErrorResult.EnsureUniqueFilename;
    }

    protected override bool ShouldApplyUniqueFilename(Job job)
    {
        return false;
    }

    protected override bool ShouldApplyMerger(Job job)
    {
        return job.ExistingFileBehavior == ExistingFileBehaviour.Merge;
    }
}
