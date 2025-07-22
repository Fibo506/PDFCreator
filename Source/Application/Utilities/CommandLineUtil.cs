using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace pdfforge.PDFCreator.Utilities;

public interface ICommandLineUtil
{
    string[] CommandLineToArgs(string commandLine);
}

public class CommandLineUtil : ICommandLineUtil
{
    /// <summary>
    ///     Uses Windows API to parse a string to a string array just as the command line args are parsed
    /// </summary>
    /// <param name="commandLine">command line string that will be parsed</param>
    /// <returns>An array of command line component strings</returns>
    public string[] CommandLineToArgs(string commandLine)
    {
        int argc;
        var argv = CommandLineToArgvW(commandLine, out argc);
        if (argv == IntPtr.Zero)
            throw new Win32Exception();
        try
        {
            var args = new string[argc];
            for (var i = 0; i < args.Length; i++)
            {
                var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                args[i] = Marshal.PtrToStringUni(p);
            }

            return args;
        }
        finally
        {
            Marshal.FreeHGlobal(argv);
        }
    }

    [DllImport("shell32.dll", SetLastError = true)]
    private static extern IntPtr CommandLineToArgvW(
        [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);
}
