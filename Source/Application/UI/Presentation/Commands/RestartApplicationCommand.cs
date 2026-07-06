using System;
using System.Diagnostics;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.Utilities.Threading;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.Commands;
public class RestartApplicationCommand : ICommand
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IThreadManager _threadManager;

    public RestartApplicationCommand(IEventAggregator eventAggregator, IThreadManager threadManager)
    {
        _eventAggregator = eventAggregator;
        _threadManager = threadManager;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {

        var exePath = Process.GetCurrentProcess().MainModule?.FileName;

        int seconds = 1;

        string cmdArgs = $"/C timeout /t {seconds} /nobreak && start \"\" \"{exePath}\"";

        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = cmdArgs,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true
        });

        _threadManager.IsStandbyDisabled = true;
        _eventAggregator.GetEvent<TryCloseApplicationEvent>().Publish();

    }

    public event EventHandler CanExecuteChanged;
}
