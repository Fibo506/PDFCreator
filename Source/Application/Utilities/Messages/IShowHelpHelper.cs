using System;

namespace pdfforge.PDFCreator.Utilities.Messages;

public interface IShowHelpHelper
{
    void CloseHelpWindow();
    void ShowHelp(string helpFolder, string topic, Func<string> getLanguage, Action<string> setLanguage);
}
