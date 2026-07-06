using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pdfforge.PDFCreator.Conversion.Settings;

namespace pdfforge.PDFCreator.UI.Presentation.UserControls.HotFolder;
internal record HotFolderAction
{
    public HotFolderActionCommand Command { get; set; }
    public HotFolderConfig Config { get; set; }

    public HotFolderAction(HotFolderActionCommand command, HotFolderConfig config)
    {
        Command = command;
        Config = config;
    }
}

internal enum HotFolderActionCommand
{
    Add,
    Remove,
    Update
}
