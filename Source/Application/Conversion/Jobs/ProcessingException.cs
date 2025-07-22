using System;

namespace pdfforge.PDFCreator.Conversion.Jobs;

public class ProcessingException : Exception
{
    public ProcessingException(string message, ErrorCode errorCode, Exception innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public ErrorCode ErrorCode { get; private set; }
}
