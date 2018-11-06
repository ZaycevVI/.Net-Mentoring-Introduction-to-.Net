using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileLibrary.Logger;

namespace FileLibrary.File
{
    public class FileOperation : IFileOperation
    {
        public async void CopyFile(string source, string destination)
        {
            var cannotAccess = true;

            do
            {
                try
                {
                    System.IO.File.Copy(source, destination);
                    SendLog($"File [{source}] was successfully copied");
                    cannotAccess = false;
                }
                catch (IOException e)
                {
                    SendLog(e.Message);
                    await Task.Delay(1000);
                }
            } while (cannotAccess);
        }

        public bool IsFileExists(string path)
        {
            if (!System.IO.File.Exists(path)) return false;

            SendLog($"File already exists: {path}.");
            return true;
        }

        public void CreateDirectoryIfNotExist(string path)
        {
            if (Directory.Exists(path)) return;

            Directory.CreateDirectory(path);
            SendLog($"Create directory by path: {path}.");
        }

        public string GetFileNameWithDate(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName) +
                   $" [{DateTime.Now:MM.dd.yyyy}]" +
                   Path.GetExtension(fileName);
        }

        public string FindDestinationDirectory(string fileName, 
            List<(Regex template, string destination)> rules, string defaultDirectory)
        {
            var rule = rules.FirstOrDefault(r => r.template.IsMatch(fileName));
            var destination = rule.destination;

            if (rule.destination != null) return destination;

            SendLog($"No rules were found for file: {fileName}.");
            destination = defaultDirectory;

            return destination;
        }

        public event SendMessageHandler SendMessage;

        private void SendLog(string msg)
        {
            SendMessage?.Invoke(msg + $" ([{DateTime.Now.ToString(CultureInfo.CurrentCulture)}])");
        }
    }
}