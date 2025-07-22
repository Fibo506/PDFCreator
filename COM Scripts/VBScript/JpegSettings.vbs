' PDFCreator COM Interface test for VBScript
' Part of the PDFCreator application
' License: GPL
' Homepage: http://www.pdfforge.org/pdfcreator
' Version: 1.1.0.0
' Created: June, 16. 2015
' Modified: May, 20. 2020 
' Author: pdfforge GmbH
' Comments: This project demonstrates the use of the COM Interface of PDFCreator.
'           This script converts a windows testpage to a jpg file.
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
fullPath = objFSO.GetSpecialFolder(TemporaryFolder) & "\TestPage_2Jpeg.jpeg"  

MsgBox "Initializing PDFCreator queue..."
PDFCreatorQueue.Initialize

MsgBox "Printing a windows testpage"
ShellObj.ShellExecute "RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n ""PDFCreator""", "", "open", 1

MsgBox "Waiting for the job to arrive at the queue..."
if not PDFCreatorQueue.WaitForJob(10) then
    MsgBox "The print job did not reach the queue within " & " 10 seconds"
else 
    MsgBox "Currently there are " & PDFCreatorQueue.Count & " job(s) in the queue"
    MsgBox "Getting job instance"
    Set printJob = PDFCreatorQueue.NextJob
    
    printJob.SetProfileByGuidOrName("JpegGuid") 'Notice that we are converting under JpegGuid
    
    MsgBox "Applying jpeg settings..."
    
    'The SetProfileSettings method allows us to change the JpegSettings of the job
	'We want 24 bit colors for our converted file
    printJob.SetProfileSetting "JpegSettings.Color", "Color24Bit"    
    printJob.SetProfileSetting "JpegSettings.Quality", "100"

    
    MsgBox "Converting under ""JpegGuid"" conversion profile"
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
