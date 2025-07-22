using System;
using System.Windows.Input;
using pdfforge.Obsidian.Trigger;
using pdfforge.PDFCreator.UI.Presentation.Events;
using pdfforge.PDFCreator.UI.Presentation.Helper;
using Prism.Events;
using Prism.Regions;

namespace pdfforge.PDFCreator.UI.Presentation.Commands;

public class NavigatePrismRegionCommand : ICommand
{
    private readonly IRegionManager _regionManager;
    private readonly IEventAggregator _aggregator;
    private PrismNavigationValueObject _prismNavigationValueObject;

    public IInteractionRequest InteractionRequest { get; }

    public NavigatePrismRegionCommand(IRegionManager regionManager, IInteractionRequest interactionRequest, IEventAggregator aggregator)
    {
        _regionManager = regionManager;
        _aggregator = aggregator;
        InteractionRequest = interactionRequest;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Navigate(string regionName, string targetView)
    {
        Execute(new PrismNavigationValueObject(regionName, targetView, null));
    }

    public void Navigate(PrismNavigationValueObject navigationObject)
    {
        Execute(navigationObject);
    }

    public void Execute(object parameter)
    {
        _prismNavigationValueObject = parameter as PrismNavigationValueObject;

        if (_prismNavigationValueObject != null)
        {
            _regionManager.RequestNavigate(_prismNavigationValueObject.RegionName, _prismNavigationValueObject.TargetView, OnNavigate);
        }
    }

    private void OnNavigate(NavigationResult navigationResult)
    {
        var navigateMainTabEvent = _aggregator.GetEvent<PrismNavigatedEvent>();
        navigateMainTabEvent.Publish(_prismNavigationValueObject);
    }

#pragma warning disable 67

    public event EventHandler CanExecuteChanged;

#pragma warning restore 67
}
