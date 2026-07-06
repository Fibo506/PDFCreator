using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace pdfforge.PDFCreator.UI.Presentation.Events;
public class ReceivedNewLicenseKeyEvent: PubSubEvent<string>
{
}
