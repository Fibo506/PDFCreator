using System;
using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Conversion.ActionsInterface;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Conversion.Processing.ITextProcessing;
using pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface.ImagesToPdf;
using pdfforge.PDFCreator.Core.Startup.StartConditions;
using pdfforge.PDFCreator.UI.COM;
using SimpleInjector;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;

[ComVisible(false)]
public class PDFCreatorCOMBootstrapper : ComBaseBootstrapper
{
    protected override void RegisterEditionSpecificPackages(Container container)
    {
        container.RegisterSingleton<IImagesToPdf, ITextImagesToPdf>();
        container.RegisterSingleton<IPdfProcessor, ITextPdfProcessor>();
        container.RegisterInstance(container.GetInstance<ITextStampAdder>);
        container.RegisterInstance(container.GetInstance<ITextPageNumbersAdder>);
        container.Register<IUserTokenExtractor, UserTokenExtractorDummy>();
    }

    protected override Type[] GetStartupConditions()
    {
        return
        [
            typeof(SpoolerRunningCondition),
            typeof(CheckSpoolFolderCondition),
            typeof(GhostscriptCondition),
            typeof(PrinterInstalledCondition)
        ];
    }

    public override void InitializeServices(Container container)
    {

    }
}
