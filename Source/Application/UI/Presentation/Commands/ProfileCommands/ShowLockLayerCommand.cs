using System;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Events;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.Commands.ProfileCommands;

internal class ShowLockLayerCommand : ICommand
{
    private readonly IEventAggregator _eventAggregator;

    public ShowLockLayerCommand(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _eventAggregator.GetEvent<ShowLockLayerEvent>().Publish();
    }

#pragma warning disable 67

    public event EventHandler CanExecuteChanged;

#pragma warning restore 67
}
