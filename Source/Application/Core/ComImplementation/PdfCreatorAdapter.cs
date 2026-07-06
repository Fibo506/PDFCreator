using System.Collections.Generic;
using System.Runtime.InteropServices;
using pdfforge.PDFCreator.Conversion.Jobs.JobInfo;
using pdfforge.PDFCreator.Core.Communication;
using pdfforge.PDFCreator.Core.DirectConversion;
using pdfforge.PDFCreator.Core.JobInfoQueue;
using pdfforge.PDFCreator.Core.Printing.Printer;
using pdfforge.PDFCreator.Core.Startup.StartConditions;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.ComImplementation;

public class PdfCreatorAdapter
{
    private readonly IDirectConversionInfFileHelper _directConversionInfFileHelper;
    private readonly IDirectImageConversionHelper _directImageConversionHelper;
    private readonly IFile _file;
    private readonly IJobInfoManager _jobInfoManager;
    private readonly IDirectConversionHelper _directConversionHelper;
    private readonly IJobInfoQueue _jobInfoQueue;
    private readonly IPipeServerManager _pipeServerManager;
    private readonly PrintFileHelperComFactory _printFileHelperComFactory;
    private readonly ISpoolFolderAccess _spoolFolderAccess;

    public PdfCreatorAdapter(
        IFile file,
        PrintFileHelperComFactory printFileHelperComFactory,
        IJobInfoQueue jobInfoQueue,
        ISpoolFolderAccess spoolFolderAccess,
        IJobInfoManager jobInfoManager,
        IDirectConversionHelper directConversionHelper,
        IDirectConversionInfFileHelper directConversionInfFileHelper,
        IDirectImageConversionHelper directImageConversionHelper,
        IPrinterHelper printerHelper,
        IPipeServerManager pipeServerManager)
    {
        PrinterHelper = printerHelper;
        _file = file;
        _printFileHelperComFactory = printFileHelperComFactory;
        _jobInfoQueue = jobInfoQueue;
        _spoolFolderAccess = spoolFolderAccess;
        _jobInfoManager = jobInfoManager;
        _directConversionHelper = directConversionHelper;
        _directConversionInfFileHelper = directConversionInfFileHelper;
        _directImageConversionHelper = directImageConversionHelper;
        _pipeServerManager = pipeServerManager;
    }

    public IPrinterHelper PrinterHelper { get; private set; }

    public bool IsInstanceRunning => _pipeServerManager.IsServerRunning();

    public void PrintFile(string path)
    {
        PrintFileSwitchingPrinters(path, false);
    }

    public void PrintFileSwitchingPrinters(string path, bool allowDefaultPrinterSwitch)
    {
        PathCheck(path);

        var printFileHelper = _printFileHelperComFactory.CreatePrintFileHelperCom();

        printFileHelper.AddFile(path, true);
        printFileHelper.AllowDefaultPrinterSwitch = allowDefaultPrinterSwitch;
        printFileHelper.PrintAll(true);
    }

    public void AddFileToQueue(string path)
    {
        PathCheck(path);

        if (!_directConversionHelper.IsImageOrDirectConversion(path))
            throw new COMException("Only .ps, .pdf and image files can be directly added to the queue.");

        if (!_spoolFolderAccess.CanAccess())
            throw new COMException("Accessing the spool folder failed.");

        string infFile;
        if (_directConversionHelper.IsImageConversion(path))
        {
            infFile = _directImageConversionHelper.TransformToInfFileDirectImageConversion(new List<string> { path }, new AppStartParameters());
        }
        else
        {
            infFile = _directConversionInfFileHelper.TransformToInfFile(path, new AppStartParameters());
        }

        if (!string.IsNullOrEmpty(infFile))
            _jobInfoQueue.Add(_jobInfoManager.ReadFromInfFile(infFile));
    }

    private void PathCheck(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new COMException("The specified path must not be empty or uninitiliazed.");

        if (!_file.Exists(path))
            throw new COMException("File with such a path doesn't exist. Please check if the specified path is correct.");
    }
}
