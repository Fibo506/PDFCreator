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

    WScript.Echo("Printing 4 times the windows test page...");
    objShell.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);
    objShell.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);
    objShell.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);
    objShell.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);

    WScript.Echo("Waiting for 4 jobs to arrive at the queue for " + 10 + " seconds");
    if (!PDFCreatorQueue.WaitForJobs(4, 10)) {
        WScript.Echo("The print job did not reach the queue within " + 10 + " seconds");
    }
    else {
        WScript.Echo("Currently there are " + PDFCreatorQueue.Count + " job(s) in the queue");

        WScript.Echo("Merging second and third job");
        //Hint: Queue starts job counting with 0!
        PDFCreatorQueue.MergeJobs(PDFCreatorQueue.GetJobByIndex(1), PDFCreatorQueue.GetJobByIndex(2));

        WScript.Echo("Starting conversion process for all jobs in the queue sequentially");
        //while loop for converting sequentially
        while (PDFCreatorQueue.Count > 0) {
            WScript.Echo("Getting next available job instance");
            var job = PDFCreatorQueue.NextJob;
            job.SetProfileByGuidOrName("DefaultGuid");

            WScript.Echo("Converting under \"DefaultGuid\" conversion profile");
            job.ConvertTo(fullPath);

            if (!job.IsFinished || !job.IsSuccessful) {
                WScript.Echo("Could not convert the file: " + fullPath);
            }
            else {
                WScript.Echo("Job finished successfully");
            }
        }
    }
    WScript.Echo("Releasing the object");
    PDFCreatorQueue.ReleaseCom();
}
catch (e) {
    WScript.Echo(e.message);
    PDFCreatorQueue.ReleaseCom();
}
