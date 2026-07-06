
using System.Linq;
using pdfforge.PDFCreator.Core.Controller.Routing;
using pdfforge.PDFCreator.UI.Presentation.Events;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.Routing;
public class ActivationLinkHandler : BaseLinkHandler
{
    private readonly IEventAggregator _eventAggregator;

    public ActivationLinkHandler(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    public override void HandlePipeLink(string link)
    {
        var result = ParseQueryString(link);
        if (result is { Action: "activate", Parameters: { } parameters }
            && parameters.FirstOrDefault(p => p.Key == "license-key") is { Value: { } licenseKeyParam })
        {
            /* There is currently no Subscription for this event.
               The actual licensing is handled in the License Condition.
               This is only the preparation to send the LicenseKey to the InvalidLicenseView */

            _eventAggregator.GetEvent<ReceivedNewLicenseKeyEvent>().Publish(licenseKeyParam);
        }
    }

    public override void HandleStartupLink(string link)
    {
    }
}
