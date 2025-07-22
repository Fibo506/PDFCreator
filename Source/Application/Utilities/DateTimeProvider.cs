using System;

namespace pdfforge.PDFCreator.Utilities;

public interface IDateTimeProvider
{
    DateTime Now();
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}
