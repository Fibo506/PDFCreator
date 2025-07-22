// This script is part of PDFCreator
// License: GPL
// Homepage: https://www.pdfforge.org

var objFSO = new ActiveXObject("Scripting.FileSystemObject");
var objShell = new ActiveXObject("Shell.Application");

var Scriptname = objFSO.GetFileName(WScript.ScriptFullname);

if (WScript.Version < 5.6) {
    WScript.Echo("You need the \"Windows Scripting Host version 5.6\" or greater!");
    WScript.Quit();
}

try {
    var PDFCreatorQueue = new ActiveXObject("PDFCreator.JobQueue");

    WScript.Echo("Initializing PDFCreator queue...");
    PDFCreatorQueue.Initialize();

    var fullPath = objFSO.GetSpecialFolder(2) + "\\TestPage.pdf";
    WScript.Echo("Setting up target path to: " + fullPath);

    WScript.Echo("Printing one windows test page...");
    objShell.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);

    WScript.Echo("Waiting for the job to arrive at the queue...");
    if (!PDFCreatorQueue.WaitForJob(10)) {
        WScript.Echo("The print job did not reach the queue within " + 10 + " seconds");
    }
    else {
        WScript.Echo("Currently there are " + PDFCreatorQueue.Count + " job(s) in the queue");
        WScript.Echo("Getting job instance");
        var job = PDFCreatorQueue.NextJob;
        job.SetProfileByGuidOrName("DefaultGuid");

        /* The SetProfileSettings method allows us to tell the job that it should attach the specified page under certain conditions */
        WScript.Echo("Applying page attachment settings...");
        //Since we want to attach a page, we have to enable it first
        job.AddAction("AttachmentPage");

        job.SetProfileSetting("AttachmentPage.File", objFSO.GetParentFolderName(WScript.ScriptFullname) + "\\AttachmentPage.pdf");
        //Notice that the first parameter here says that we want to attach a file and the second specifies the file. It is very important that
        //the file is in pdf-Format otherwise exceptions will occur

        WScript.Echo("Converting under \"DefaultGuid\" conversion profile");
        job.ConvertTo(fullPath);

        if (!job.IsFinished || !job.IsSuccessful) {
            WScript.Echo("Could not convert the file: " + fullPath);
        }
        else {
            WScript.Echo("Job finished successfully");
        }
    }
    WScript.Echo("Releasing the object");
    PDFCreatorQueue.ReleaseCom();
}

catch (e) {
    WScript.Echo(e.message);
    PDFCreatorQueue.ReleaseCom();
}
