using System;

namespace pdfforge.PDFCreator.Core.Workflow.Exceptions;

public class WorkflowException : Exception
{
    public WorkflowException(string message)
        : base(message)
    {
    }
}
