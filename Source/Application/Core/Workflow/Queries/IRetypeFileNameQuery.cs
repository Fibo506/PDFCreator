using pdfforge.PDFCreator.Conversion.Jobs.Query;
using pdfforge.PDFCreator.Conversion.Settings.Enums;

namespace pdfforge.PDFCreator.Core.Workflow.Queries;

public enum RetypeReason
{
    InvalidRootedPath,
    CopyError
}

public interface IRetypeFileNameQuery
{
    QueryResult<string> RetypeFileNameQuery(string filename, OutputFormat outputFormat, RetypeReason retypeReason);
}
