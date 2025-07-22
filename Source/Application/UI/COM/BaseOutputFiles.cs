using System.Collections.Generic;

namespace pdfforge.PDFCreator.UI.COM;

public interface IBaseOutputFiles
{
    int Count { get; }

    string GetFilename(int index);
}

public class BaseOutputFiles : IBaseOutputFiles
{
    private readonly IList<string> _outputFiles;

    /// <summary>
    ///     Initializing private list with provided list
    /// </summary>
    /// <param name="outputFileList">Provided list</param>
    public BaseOutputFiles(IList<string> outputFileList)
    {
        _outputFiles = outputFileList;
    }

    /// <summary>
    ///     Returns the number of filenames in the list
    /// </summary>
    public int Count
    {
        get { return _outputFiles.Count; }
    }

    /// <summary>
    /// </summary>
    /// <param name="index">The position of filename in the list</param>
    /// <returns>The filename corresponding to indexed list value </returns>
    public string GetFilename(int index)
    {
        return _outputFiles[index];
    }
}
