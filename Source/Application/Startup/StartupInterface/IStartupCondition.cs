namespace pdfforge.PDFCreator.Core.StartupInterface;

public interface IStartupCondition
{
    bool CanRequestUserInteraction { get; }

    StartupConditionResult Check();
}
