using System;
using System.Collections.Generic;
using pdfforge.PDFCreator.Conversion.Jobs.Jobs;

namespace pdfforge.PDFCreator.UI.Presentation;

public interface IShellManager
{
    void ShowMainShell();

    void ShowPrintJobShell(Job job);

    void MainShellToFront();

    void SetPrintJobShellRegionToViewRegister(List<(string, Type)> regionToViewRegister);

    void SetMainShellRegionToViewRegister(List<(string, Type)> regionToViewRegister);

    bool PrintJobShellIsOpen { get; }
    bool MainShellIsOpen { get; }
}
