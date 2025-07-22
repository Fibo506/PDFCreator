using System;
using System.Text.RegularExpressions;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Utilities.Tokens;

public interface IFileIndexHelper
{
    string ReplaceFileIndex(string inputString, int numberOfFiles, int currentFileIndex = 1);
}

public class FileIndexHelper : IFileIndexHelper
{
    public const string FileIndexRegEx = $@"(\{{{TokenNames.FileIndex}(%([\s\S]*?))*\}})";

    public string ReplaceFileIndex(string inputPath, int numberOfFiles, int currentFileIndex = 1)
    {
        if (inputPath == null)
            return null;

        if (numberOfFiles < 1)
            numberOfFiles = 1;

        //keep a consistent number of digits for the file index e.g. D3 for >=100 files (001 -> 100) 
        var defaultIndexFormat = "D" + (int)Math.Floor(Math.Log10(numberOfFiles) + 1);

        //Handle FileIndex Token
        var match = Regex.Match(inputPath, FileIndexRegEx);
        if (match.Success)
        {
            //Use the optional format stored in Group[2]
            var format = match.Groups[2].Success ? match.Groups[3].Value : defaultIndexFormat;
            return inputPath.Replace(match.Groups[0].Value, currentFileIndex.ToString(format));
        }

        //Do not apply file index if there is only one file 
        if (numberOfFiles == 1)
            return inputPath;

        //Append file index for file name  
        var outputDir = PathSafe.GetDirectoryName(inputPath) ?? "";
        var filenameBase = PathSafe.GetFileNameWithoutExtension(inputPath) ?? "output";
        var pathBase = PathSafe.Combine(outputDir, filenameBase);
        var extension = PathSafe.GetExtension(inputPath);

        return pathBase + currentFileIndex.ToString(defaultIndexFormat) + extension;
    }
}
