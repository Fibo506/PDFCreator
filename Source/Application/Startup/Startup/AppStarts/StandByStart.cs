using System;
using pdfforge.PDFCreator.Core.Startup.HotFolder;
using pdfforge.PDFCreator.Utilities.Threading;

namespace pdfforge.PDFCreator.Core.Startup.AppStarts;

public class StandByStart : MaybePipedStart
{
    private readonly IThreadManager _threadManager;
    private readonly IHotFolderManager _hotFolderManager;

    public StandByStart(IMaybePipedApplicationStarter maybePipedApplicationStarter, IThreadManager threadManager, IHotFolderManager hotFolderManager) : base(maybePipedApplicationStarter)
    {
        _threadManager = threadManager;
        _hotFolderManager = hotFolderManager;
    }

    protected override string ComposePipeMessage()
    {
        return "";
    }

    protected override bool StartApplication()
    {
        _threadManager.HotStandbyDuration = TimeSpan.FromSeconds(-1);
        _hotFolderManager.StartAll();
        return true;
    }
}
