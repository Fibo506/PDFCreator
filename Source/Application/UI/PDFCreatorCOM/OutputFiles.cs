using System.Collections.Generic;
using System.Runtime.InteropServices;
using pdfforge.PDFCreator.UI.COM;

namespace pdfforge.PDFCreator.UI.PDFCreatorCOM;


[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
[Guid("15D1FBEA-9BC9-4B55-8D1E-295E8ADCCD42")]
public interface IOutputFiles
{
    int Count { get; }
    string GetFilename(int index);
}


[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[Guid("071A256A-A4BA-417F-B64F-B3F3E1600B8A")]
public class OutputFiles : IOutputFiles
{
    private readonly BaseOutputFiles _baseOutputFiles;

    internal OutputFiles(BaseOutputFiles baseOutputFiles)
    {
        _baseOutputFiles = baseOutputFiles;
    }

    protected internal OutputFiles(IList<string> outputFileList)
    {
        _baseOutputFiles = new BaseOutputFiles(outputFileList);
    }

    public int Count => _baseOutputFiles.Count;

    public string GetFilename(int index)
    {
        return _baseOutputFiles.GetFilename(index);
    }
}
