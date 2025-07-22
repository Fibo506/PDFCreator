using System;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Events;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.Commands.ProfileCommands;

public class HideLockLayerCommand : ICommand
{
    private readonly IEventAggregator _eventAggregator;

    public HideLockLayerCommand(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _eventAggregator.GetEvent<HideLockLayerEvent>().Publish();
    }

#pragma warning disable 67

    public event EventHandler CanExecuteChanged;

#pragma warning restore 67
}
