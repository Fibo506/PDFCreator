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

    var fullPath = objFSO.GetSpecialFolder(2) + "\\TestPage.jpg";
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
        job.SetProfileByGuidOrName("JpegGuid");	//Notice that we are converting under JpegGuid.

        WScript.Echo("Applying jpeg settings...");
        /* The SetProfileSettings method allows us to change the JpegSettings of the job*/
        //We want 24 bit colors for our converted file
        job.SetProfileSetting("JpegSettings.Color", "Color24Bit");
        //We want the best quality possible for the converted file
        job.SetProfileSetting("JpegSettings.Quality", "100");

        WScript.Echo("Converting under \"JpegGuid\" conversion profile");
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
