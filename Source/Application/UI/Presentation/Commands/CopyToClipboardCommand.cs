using System;
using System.Windows.Input;
using pdfforge.PDFCreator.UI.Presentation.Helper;

namespace pdfforge.PDFCreator.UI.Presentation.Commands;

public class CopyToClipboardCommand : ICommand
{
    private readonly IClipboardService _clipboardService;

    public CopyToClipboardCommand(IClipboardService clipboardService)
    {
        _clipboardService = clipboardService;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        try
        {
            _clipboardService.SetDataObject(parameter);
        }
        catch { }
    }

#pragma warning disable CS0067

    public event EventHandler CanExecuteChanged;

#pragma warning restore CS0067
}
