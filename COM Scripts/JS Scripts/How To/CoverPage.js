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

        WScript.Echo("Applying cover page settings...");
        /* The SetProfileSettings method allows us to tell the job that it should be send after conversion */
        //Since we want to add a cover page, we have to enable it first
        job.AddAction("CoverPage"); 		
		//The file needs to be a PDF, otherwise exceptions will occur
		job.SetProfileSetting("CoverPage.File",
		objFSO.GetParentFolderName(WScript.ScriptFullname) + "\\CoverPage.pdf");

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
