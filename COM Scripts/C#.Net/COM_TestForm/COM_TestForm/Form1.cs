/*
 * PDFCreator COM samples for C#
 * Part of the PDFCreator application
 * License: GPL
 * Homepage: http://www.pdfforge.org/pdfcreator
 * .Net Framework: 4.7.2
 * Version: 1.1.0.0
 * Created: May, 25. 2020
 * Modified: May, 25. 2020
 * Author: pdfforge GmbH
 * Comments: This project demonstrates the use of the COM Interface of PDFCreator.
             There are 5 different kinds of usage presented.
             Further usage presentation is only available in the JavaScript directory.
 * Note: When executed in release mode paths have to be modified.
 */

using pdfforge.PDFCreator.UI.ComWrapper;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using pdfforge.PDFCreator.UI.ComWrapper;
//IMPORTANT: Add a reference to the PDFCreator.ComWrapper.dll

namespace COM_TestForm
{
    public partial class Form1 : Form
    {
        private bool _isTypeInitialized;

        public Form1()
        {
            InitializeComponent();
            UpdateStatus("Program started");
        }

        private JobQueue CreateQueue()
        {
            // This needs to be done once to make the ComWrapper work reliably.
            if (!_isTypeInitialized)
            {
                Type queueType = Type.GetTypeFromProgID("PDFCreator.JobQueue");
                Activator.CreateInstance(queueType);
                _isTypeInitialized = true;
            }

            return new JobQueue();
        }

        private void testPage_btn_Click(object sender, EventArgs e)
        {
            const string sampleName = "TestPage2PDF";

            UpdateStatus(string.Format(Messages.SampleStarted, sampleName));

            var captionName = Messages.DefaultMessageCaption + ": " + sampleName;
            var convertedFilePath = Path.Combine(Path.GetTempPath(), sampleName + ".pdf");

            var jobQueue = CreateQueue();

            try
            {
                MessageBox.Show(Messages.Initializing, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.Initialize();

                MessageBox.Show(Messages.PrintingTestPage, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrintWindowsTestPage();

                const int waitTimeSeconds = 10;
                if (!jobQueue.WaitForJob(waitTimeSeconds))
                {
                    MessageBox.Show(string.Format(Messages.JobDidNotArrive, waitTimeSeconds), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Messages.GettingJobInstance, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var printJob = jobQueue.NextJob;

                    const string profile = "DefaultGuid";
                    printJob.SetProfileByGuidOrName(profile);

                    MessageBox.Show(string.Format(Messages.ConvertingWithProfile, profile), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.ConvertTo(convertedFilePath);

                    if (!printJob.IsFinished || !printJob.IsSuccessful)
                    {
                        UpdateStatus(Messages.CouldNotConvert);
                        MessageBox.Show(Messages.CouldNotConvert, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus(string.Format(Messages.ConversionSuccessfulFile1, convertedFilePath));
                        MessageBox.Show(string.Format(Messages.ConversionSuccessfulFile2, convertedFilePath), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format(Messages.Error, err.Message), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MessageBox.Show(Messages.ReleasingQueueObject, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.ReleaseCom();
            }
        }

        private void PrintWindowsTestPage()
        {
            Type shellObj = Type.GetTypeFromProgID("Shell.Application");
            dynamic shellInst = Activator.CreateInstance(shellObj);

            shellInst.ShellExecute("RUNDLL32.exe", "PRINTUI,PrintUIEntry /k /n \"PDFCreator\"", "", "open", 1);
        }

        private void jpegSettings_btn_Click(object sender, EventArgs e)
        {
            const string sampleName = "TestPage2Jpeg";

            UpdateStatus(string.Format(Messages.SampleStarted, sampleName));

            var captionName = Messages.DefaultMessageCaption + ": " + sampleName;
            var convertedFilePath = Path.Combine(Path.GetTempPath(), sampleName + ".jpg");

            var jobQueue = CreateQueue();

            try
            {
                MessageBox.Show(Messages.Initializing, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.Initialize();

                MessageBox.Show(Messages.PrintingTestPage, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrintWindowsTestPage();

                const int waitTimeSeconds = 10;
                if (!jobQueue.WaitForJob(waitTimeSeconds))
                {
                    MessageBox.Show(string.Format(Messages.JobDidNotArrive, waitTimeSeconds), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Messages.GettingJobInstance, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var printJob = jobQueue.NextJob;

                    const string profile = "JpegGuid";
                    printJob.SetProfileByGuidOrName(profile);

                    MessageBox.Show("Applying jpeg settings", captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.SetProfileSetting("JpegSettings.Color", "Color24Bit");
                    printJob.SetProfileSetting("JpegSettings.Quality", "100");

                    MessageBox.Show(string.Format(Messages.ConvertingWithProfile, profile), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.ConvertTo(convertedFilePath);

                    if (!printJob.IsFinished || !printJob.IsSuccessful)
                    {
                        UpdateStatus(Messages.CouldNotConvert);
                        MessageBox.Show(Messages.CouldNotConvert, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus(string.Format(Messages.ConversionSuccessfulFile1, convertedFilePath));
                        MessageBox.Show(string.Format(Messages.ConversionSuccessfulFile2, convertedFilePath), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format(Messages.Error, err.Message), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MessageBox.Show(Messages.ReleasingQueueObject, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.ReleaseCom();
            }
        }

        private void mergedFiles_btn_Click(object sender, EventArgs e)
        {
            const string sampleName = "MergedMultipleFiles2Tif";

            UpdateStatus(string.Format(Messages.SampleStarted, sampleName));

            var captionName = Messages.DefaultMessageCaption + ": " + sampleName;
            var convertedFilePath = Path.Combine(Path.GetTempPath(), sampleName + ".tif");

            var jobQueue = CreateQueue();

            try
            {
                MessageBox.Show(Messages.Initializing, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.Initialize();

                var countTestPages = 3;
                MessageBox.Show(string.Format("Printing {0} windows test pages ...", countTestPages), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                for (int i = 0; i < countTestPages; i++)
                    PrintWindowsTestPage();

                const int waitTimeSeconds = 15;
                if (!jobQueue.WaitForJobs(countTestPages, waitTimeSeconds))
                {
                    MessageBox.Show(string.Format(Messages.JobDidNotArrive, waitTimeSeconds), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("Merging all available jobs", captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    jobQueue.MergeAllJobs();

                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Messages.GettingJobInstance, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var printJob = jobQueue.NextJob;

                    const string profile = "DefaultGuid";
                    printJob.SetProfileByGuidOrName(profile);

                    //Change the output format of the current conversion profile
                    //to .tif - this holds only for this job
                    printJob.SetProfileSetting("OutputFormat", "Tif");

                    MessageBox.Show(string.Format("Converting with profile \"{0}\" but with .tif as output format.", profile), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.ConvertTo(convertedFilePath);

                    if (!printJob.IsFinished || !printJob.IsSuccessful)
                    {
                        UpdateStatus(Messages.CouldNotConvert);
                        MessageBox.Show(Messages.CouldNotConvert, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus(string.Format(Messages.ConversionSuccessfulFile1, convertedFilePath));
                        MessageBox.Show(string.Format(Messages.ConversionSuccessfulFile2, convertedFilePath), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format(Messages.Error, err.Message), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MessageBox.Show(Messages.ReleasingQueueObject, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.ReleaseCom();
            }
        }

        private void coverPage_btn_Click(object sender, EventArgs e)
        {
            const string sampleName = "CoverPage";

            UpdateStatus(string.Format(Messages.SampleStarted, sampleName));

            var captionName = Messages.DefaultMessageCaption + ": " + sampleName;
            var convertedFilePath = Path.Combine(Path.GetTempPath(), sampleName + ".pdf");

            var jobQueue = CreateQueue();

            var assemblyDir = Assembly.GetExecutingAssembly().Location;
            var coverPagePath = assemblyDir.Replace("\\bin\\Debug\\COM_TestForm.exe", "\\FilesForTests\\CoverPage.pdf");

            try
            {
                MessageBox.Show(Messages.Initializing, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.Initialize();

                MessageBox.Show(Messages.PrintingTestPage, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrintWindowsTestPage();

                const int waitTimeSeconds = 10;
                if (!jobQueue.WaitForJob(waitTimeSeconds))
                {
                    MessageBox.Show(string.Format(Messages.JobDidNotArrive, waitTimeSeconds), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Messages.GettingJobInstance, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var printJob = jobQueue.NextJob;

                    const string profile = "DefaultGuid";
                    printJob.SetProfileByGuidOrName(profile);
                    MessageBox.Show("Applying cover page settings", captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.AddAction("CoverPage");
                    printJob.SetProfileSetting("CoverPage.File", coverPagePath);

                    MessageBox.Show(string.Format(Messages.ConvertingWithProfile, profile), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.ConvertTo(convertedFilePath);

                    if (!printJob.IsFinished || !printJob.IsSuccessful)
                    {
                        UpdateStatus(Messages.CouldNotConvert);
                        MessageBox.Show(Messages.CouldNotConvert, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus(string.Format(Messages.ConversionSuccessfulFile1, convertedFilePath));
                        MessageBox.Show(string.Format(Messages.ConversionSuccessfulFile2, convertedFilePath), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format(Messages.Error, err.Message), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MessageBox.Show(Messages.ReleasingQueueObject, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.ReleaseCom();
            }
        }

        private void backgroundPage_btn_Click(object sender, EventArgs e)
        {
            const string sampleName = "BackgroundPage";

            UpdateStatus(string.Format(Messages.SampleStarted, sampleName));

            var captionName = Messages.DefaultMessageCaption + ": " + sampleName;
            var convertedFilePath = Path.Combine(Path.GetTempPath(), sampleName + ".pdf");

            var jobQueue = CreateQueue();

            var assemblyDir = Assembly.GetExecutingAssembly().Location;
            var bkgroundPagePath = assemblyDir.Replace("\\bin\\Debug\\COM_TestForm.exe", "\\FilesForTests\\BackgroundPage.pdf");

            try
            {
                MessageBox.Show(Messages.Initializing, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.Initialize();

                MessageBox.Show(Messages.PrintingTestPage, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                PrintWindowsTestPage();

                const int waitTimeSeconds = 10;
                if (!jobQueue.WaitForJob(waitTimeSeconds))
                {
                    MessageBox.Show(string.Format(Messages.JobDidNotArrive, waitTimeSeconds), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Format(Messages.JobsInQueue, jobQueue.Count), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(Messages.GettingJobInstance, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var printJob = jobQueue.NextJob;
                    var list = printJob.GetProfileListSetting("ActionOrder");
                    const string profile = "DefaultGuid";
                    printJob.SetProfileByGuidOrName(profile);

                    MessageBox.Show("Applying background page settings", captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.AddActionToPosition("BackgroundPage", 1);
                    list = printJob.GetProfileListSetting("ActionOrder");
                    printJob.SetProfileSetting("BackgroundPage.Repetition", "RepeatAllPages");
                    printJob.SetProfileSetting("BackgroundPage.File", bkgroundPagePath);

                    MessageBox.Show(string.Format(Messages.ConvertingWithProfile, profile), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    printJob.ConvertTo(convertedFilePath);

                    if (!printJob.IsFinished || !printJob.IsSuccessful)
                    {
                        UpdateStatus(Messages.CouldNotConvert);
                        MessageBox.Show(Messages.CouldNotConvert, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus(string.Format(Messages.ConversionSuccessfulFile1, convertedFilePath));
                        MessageBox.Show(string.Format(Messages.ConversionSuccessfulFile2, convertedFilePath), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format(Messages.Error, err.Message), captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                MessageBox.Show(Messages.ReleasingQueueObject, captionName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                jobQueue.ReleaseCom();
            }
        }

        private void UpdateStatus(string status)
        {
            toolStripStatusLabel1.Text = status;
        }

        private void statusStrip1_Resize(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Width = statusStrip1.Width;
        }
    }

    public static class Messages
    {
        public const string SampleStarted = "Sample \"{0}\" started";
        public const string Initializing = "Initializing the job queue";
        public const string PrintingTestPage = "Printing windows test page ...";
        public const string JobDidNotArrive = "The job didn't arrive within {0} seconds.";
        public const string JobsInQueue = "Currently there are {0} job(s) in the queue.";
        public const string GettingJobInstance = "Getting job instance";
        public const string ConvertingWithProfile = "Converting with profile \"{0}\"";
        public const string ConversionSuccessfulFile1 = "The conversion was succesful! [File: {0}]";
        public const string ConversionSuccessfulFile2 = "The conversion was succesful!\r\n\r\nThe file was saved to: {0}";
        public const string CouldNotConvert = "Could not convert!";
        public const string Error = "An error occured: {0}";
        public const string ReleasingQueueObject = "Releasing the queue object";

        public const string DefaultMessageCaption = "PDFCreator c# com sample";
    }
}