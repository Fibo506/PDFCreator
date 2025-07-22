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

    //Sets up the path where the converted pdf file should be saved in
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
        // Getting Job instance
        var job = PDFCreatorQueue.NextJob;
        job.SetProfileByGuidOrName("DefaultGuid");

         // Notice here we specifies the files. It is very important that the files are in pdf-Format otherwise exceptions will occur
        job.AddAction("CoverPage");
        job.SetProfileSetting("CoverPage.File", objFSO.GetParentFolderName(WScript.ScriptFullname) + "\\CoverPage.pdf");
        
        job.AddAction("BackgroundPage");
        job.SetProfileSetting("BackgroundPage.File", objFSO.GetParentFolderName(WScript.ScriptFullname) + "\\BackgroundPage.pdf");
        
        job.AddAction("AttachmentPage");
        job.SetProfileSetting("AttachmentPage.File", objFSO.GetParentFolderName(WScript.ScriptFullname) + "\\AttachmentPage.pdf");

        // a helpful method to list all parmeters in an ProfileSettingList
        function listActions(list) {
            var length = list.Count;
            var actions = "Number of active actions: " + list.Count + "\n\n";
            for (var i = 0; i < length; i++) {
                actions = actions + i + "." + list.item(i) + "\n";
            }
            WScript.Echo(actions);
        }

        // getting a copied list of all active job in the selected profile, the list elements will be string
        // keep in mind this list is a copy of the "real" actionOrder list, changes will not have any
        // effect on the profile itself
        var activeActions = job.GetProfileListSetting("ActionOrder");
        listActions(activeActions);

        // actions can be removed by using job.RemoveAction
        job.RemoveAction("CoverPage");
        listActions(job.GetProfileListSetting("ActionOrder"));

        // actions can also be added to defined positions - index starts at zero
        // the following line restores the original order
        job.AddActionToPosition("CoverPage", 0);
        listActions(job.GetProfileListSetting("ActionOrder"));

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
