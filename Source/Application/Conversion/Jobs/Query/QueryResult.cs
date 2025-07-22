namespace pdfforge.PDFCreator.Conversion.Jobs.Query;

public class QueryResult<T>
{
    public QueryResult()
    {
    }

    public QueryResult(bool success, T data)
    {
        Success = success;
        Data = data;
    }

    public bool Success { get; set; }
    public T Data { get; set; }
}
