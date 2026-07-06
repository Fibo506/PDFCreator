using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.Conversion.Processing.PdfProcessingInterface;

public interface ILibPathHelper
{
    void AddPlatformLibPath();
}

public class LibPathHelper : ILibPathHelper
{
    private readonly IAssemblyHelper _assemblyHelper;
    private readonly IOsHelper _osHelper;

    public LibPathHelper(IAssemblyHelper assemblyHelper, IOsHelper osHelper)
    {
        _assemblyHelper = assemblyHelper;
        _osHelper = osHelper;
    }

    public void AddPlatformLibPath()
    {
        var libPath = _assemblyHelper.GetAssemblyDirectory() + "\\lib\\";
        libPath += _osHelper.Is64BitProcess ? "x64" : "x86";
        _osHelper.AddDllDirectorySearchPath(libPath);
    }
}
