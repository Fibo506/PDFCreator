using Newtonsoft.Json;

namespace pdfforge.PDFCreator.Core.Services.JobHistory;

public class HistoricFile
{
    public string Path { get; set; }
    public string FileName { get; set; }
    public string Directory { get; set; }
    public string Hash { get; set; }

    [JsonConstructor]
    public HistoricFile(string path, string fileName, string directory, string hash)
    {
        Path = path;
        FileName = fileName;
        Directory = directory;
        Hash = hash;
    }
}
