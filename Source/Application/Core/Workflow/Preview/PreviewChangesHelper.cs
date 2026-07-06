using System.Collections.Generic;
using System.Linq;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.Core.Workflow;

public interface IPreviewChangesHelper
{
    void ApplyPreviewChanges(Job job);
    bool ArePreviewChangesNeeded(Job job);
}

public class PreviewChangesHelper : IPreviewChangesHelper
{
    private readonly IPdfProcessor _pdfProcessor;
    private readonly IPreviewManager _previewManager;

    public PreviewChangesHelper(IPdfProcessor pdfProcessor, IPreviewManager previewManager)
    {
        _pdfProcessor = pdfProcessor;
        _previewManager = previewManager;
    }

    public void ApplyPreviewChanges(Job job)
    {
        var previewPages = _previewManager.GetTotalPreviewPages(job.JobInfo).GetAwaiter().GetResult();
        var previewPageMappings = MapToPageMapping(previewPages);
        _previewManager.AbortAndCleanUpPreview(job.JobInfo.SourceFiles);

        _pdfProcessor.ApplyPreviewChanges(job, previewPageMappings);
    }

    public bool ArePreviewChangesNeeded(Job job)
    {
        if (job.Profile.AutoSave.Enabled)
            return false;

        var previewPages = _previewManager.GetTotalPreviewPages(job.JobInfo).GetAwaiter().GetResult();

        var anyPageManipulated = previewPages.Any(p => p.RotationAngle != 0 || p.IsExcluded);

        if (!anyPageManipulated)
        {
            _previewManager.AbortAndCleanUpPreview(job.JobInfo.SourceFiles);
        }

        return anyPageManipulated;
    }

    protected IList<PageMapping> MapToPageMapping(IList<PreviewPage> previewPages)
    {
        var pageMappings = new List<PageMapping>();
        foreach (var previewPage in previewPages)
        {
            var pageMapping = new PageMapping(previewPage.PageNumber, previewPage.SourcePageNumber, previewPage.RotationAngle, previewPage.IsExcluded);
            pageMappings.Add(pageMapping);
        }
        return pageMappings;
    }
}

public class DisabledPreviewChangesHelper : IPreviewChangesHelper
{
    public void ApplyPreviewChanges(Job job)
    {
    }

    public bool ArePreviewChangesNeeded(Job job)
    {
        return false;
    }
}
