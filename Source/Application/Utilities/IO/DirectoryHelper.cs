using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Utilities.IO;

public class DirectoryHelper : IDirectoryHelper
{
    private readonly IDirectory _directoryWrap;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private List<string> _createdDirectories;

    public DirectoryHelper(IDirectory directoryWrap)
    {
        _directoryWrap = directoryWrap;
        _createdDirectories = new List<string>();
    }

    public virtual List<string> CreatedDirectories
    {
        get { return _createdDirectories; }
    }

    /// <summary>
    ///     Returns a list that contains all parent directories of the directory
    /// </summary>
    public List<string> GetDirectoryTree(string directory)
    {
        directory = directory.TrimEnd('\\');
        var directoryTree = new List<string>();

        while (directory?.Length > 3) //while directory has parent that is not the root drive
        {
            directoryTree.Add(directory);
            directory = Path.GetDirectoryName(directory);
        }
        return directoryTree;
    }

    /// <summary>
    ///     Creates the directory and stores in "CreatedDirectories" which parent directories hab to be created
    /// </summary>
    /// <returns>false if creating directory throws exception</returns>
    public bool CreateDirectory(string directory)
    {
        try
        {
            var directoryTree = GetDirectoryTree(directory);
            directoryTree = directoryTree.OrderBy(x => x).ToList(); //start with smallest

            foreach (var path in directoryTree)
            {
                if (!_directoryWrap.Exists(path))
                {
                    try
                    {
                        _directoryWrap.CreateDirectory(path);
                        _logger.Debug("Created directory " + path);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("Exception while creating \"" + path + "\"\r\n" + ex.Message);
                        return false;
                    }
                    _createdDirectories.Add(path);
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     Deletes all parent directories that had to be created to reach the requested directory.
    ///     If a parent directory contains files, it will not be deleted, likewise further parent directories.
    /// </summary>
    /// <returns>false if deleting directory throws exception</returns>
    public bool DeleteCreatedDirectories()
    {
        _createdDirectories = CreatedDirectories.OrderByDescending(x => x).ToList(); //start with tallest
        foreach (var createdDirectory in _createdDirectories)
        {
            if (!_directoryWrap.Exists(createdDirectory))
                continue;

            if (_directoryWrap.EnumerateFileSystemEntries(createdDirectory).Any())
                continue;

            try
            {
                _directoryWrap.Delete(createdDirectory);
                _logger.Debug("Deleted previously created but unused directory " + createdDirectory);
            }
            catch (Exception ex)
            {
                _logger.Warn("Exception while deleting created but unused directory \"" + createdDirectory + "\"\r\n" + ex.Message);
                return false;
            }
        }
        return true;
    }
}
