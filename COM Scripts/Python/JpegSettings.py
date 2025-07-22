'''
 PDFCreator COM Interface test for Python
 Part of the PDFCreator application
 License: GPL
 Homepage: http://www.pdfforge.org/pdfcreator
 Python Version: 3.7
 Version: 1.1.0.0
 Created: May, 25. 2020
 Modified: May, 25. 2020 
Comments: This project demonstrates the use of the COM Interface of PDFCreator.
           This script converts a windows testpage to a jpg file.
 Note: More usage examples then in the Python directory can be found in the JavaScript directory only.
'''

#for COM support
import tempfile
import os
import win32com.client as w32c 
import ctypes
import tkinter.messagebox as mbox #IMPORTANT: CHECK IF tkinter MODULE IST INSTALLED! Command: python -m tkinter

ShellObj = w32c.Dispatch("Shell.Application")
PDFCreatorQueue = w32c.Dispatch("PDFCreator.JobQueue")
Dir = tempfile.gettempdir()
fullPath = Dir + "\TestPage_2Jpeg.jpg" 

mbox.showinfo("","Initializing PDFCreator queue...")
PDFCreatorQueue.Initialize()

mbox.showinfo("","Printing a windows testpage")
ShellObj.ShellExecute("RUNDLL32.exe", "PRINTUI.DLL,PrintUIEntry /k /n ""PDFCreator""", "", "open", 1)

mbox.showinfo("","Waiting for the job to arrive at the queue...")
if(not PDFCreatorQueue.WaitForJob(10)):
    mbox.showinfo("","The print job did not reach the queue within 10 seconds")
else:
    mbox.showinfo("","Currently there are " + str(PDFCreatorQueue.Count) + " job(s) in the queue")
    mbox.showinfo("","Getting job instance")
    printJob = PDFCreatorQueue.NextJob
    
    printJob.SetProfileByGuidOrName("JpegGuid") #Notice that we are converting under JpegGuid
    
    mbox.showinfo("","Applying jpeg settings...")
    
    #The SetProfileSettings method allows us to change the JpegSettings of the job
	#We want 24 bit colors for our converted file
    printJob.SetProfileSetting("JpegSettings.Color", "Color24Bit")    
    printJob.SetProfileSetting("JpegSettings.Quality", "100")

    
    mbox.showinfo("","Converting under ""JpegGuid"" conversion profile")
    printJob.ConvertTo(fullPath)
    
    if(not printJob.IsFinished or not printJob.IsSuccessful):
	    mbox.showinfo("","Could not convert")
    else:
	    mbox.showinfo("","Job finished successfully.\n\nFile saved to: " + fullPath)
mbox.showinfo("","Releasing the object")
PDFCreatorQueue.ReleaseCom()
