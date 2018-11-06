using System.Collections.Generic;
using System.Text.RegularExpressions;
using FileLibrary.Logger;

namespace FileLibrary.File
{
    public interface IFileOperation : ILogNotifier
    {
        void CopyFile(string source, string destination);
        bool IsFileExists(string path);
        void CreateDirectoryIfNotExist(string path);
        string GetFileNameWithDate(string fileName);
        string FindDestinationDirectory(string fileName,
            List<(Regex template, string destination)> rules, string defaultDirectory);
    }
}