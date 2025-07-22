' PDFCreator COM Interface test for VBScript
' Part of the PDFCreator application
' License: GPL
' Homepage: http://www.pdfforge.org/pdfcreator
' Version: 1.1.0.0
' Created: June, 16. 2015
' Modified: May, 20. 2020 
' Author: pdfforge GmbH
' Comments: This project demonstrates the use of the COM Interface of PDFCreator.
'           This script converts 3 windows testpage to one .tif file.
' Note: More usage examples then in the VBScript directory can be found in the JavaScript directory only.

Dim ShellObj, PDFCreatorQueue, scriptName, fullPath, printJob, objFSO, tmp
Const TemporaryFolder = 2

if (WScript.Version < 5.6) then
    MsgBox "You need the Windows Scripting Host version 5.6 or greater!"
    WScript.Quit
end if

Set objFSO = CreateObject("Scripting.FileSystemObject")
Set ShellObj = CreateObject("Shell.Application")
Set PDFCreatorQueue = CreateObject("PDFCreator.JobQueue")
fullPath = objFSO.GetSpecialFolder(TemporaryFolder) & "\TestMerge_2Tif.tif "  

MsgBox "Initializing PDFCreator queue..."
PDFCreatorQueue.Initialize

MsgBox "Printing 3 windows testpages"
ShellObj.ShellExecute "RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n ""PDFCreator""", "", "open", 1
ShellObj.ShellExecute "RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n ""PDFCreator""", "", "open", 1
ShellObj.ShellExecute "RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n ""PDFCreator""", "", "open", 1

MsgBox "Waiting for the 3 jobs to arrive at the queue for 15 seconds..."
if not PDFCreatorQueue.WaitForJobs(3, 15) then
    MsgBox "The print job did not reach the queue within " & " 15 seconds"
else 
    MsgBox "Currently there are " & PDFCreatorQueue.Count & " job(s) in the queue"
    
    MsgBox "Merging all available jobs now"
    PDFCreatorQueue.MergeAllJobs
    
    MsgBox "Now there are " & PDFCreatorQueue.Count & " job(s) in the queue"
    MsgBox "Getting job instance"
    Set printJob = PDFCreatorQueue.NextJob
    
    printJob.SetProfileByGuidOrName("DefaultGuid")
    
    MsgBox "Setting OutputFormat to tif"
    printJob.SetProfileSetting "OutputFormat", "Tif" 
    
    MsgBox "Converting under ""DefaultGuid"" conversion profile but with .Tif as output format"
    printJob.ConvertTo(fullPath)
    
    if (not printJob.IsFinished or not printJob.IsSuccessful) then
		MsgBox "Could not convert the file: " & fullPath
	else
        MsgBox "File created: " & fullPath
		MsgBox "Job finished successfully"
    end if
end if

MsgBox "Releasing the object"
PDFCreatorQueue.ReleaseCom