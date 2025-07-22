using System;

namespace pdfforge.PDFCreator.Core.Workflow.Exceptions;

public class AbortWorkflowException : Exception
{
    public AbortWorkflowException(string message)
        : base(message)
    {
    }
}
