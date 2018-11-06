using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using FileLibrary.Configuration;
using FileLibrary.Logger;
using Directory = FileLibrary.Configuration.Directory;

namespace FileLibrary.File
{
    public class FileNotifier : IFileNotifier
    {
        private readonly List<(Regex template, string destination)> _rules;
        private readonly List<FileSystemWatcher> _fileSystemWatchers;
        private readonly SettingSection _settingSection;
        private readonly IFileOperation _fileOperation;

        public FileNotifier(IConfiguration configuration, IFileOperation fileOperation)
        {
            _fileSystemWatchers = new List<FileSystemWatcher>();
            _settingSection = configuration.SettingSection;
            _fileOperation = fileOperation;
            _rules = configuration.GetRules();
            InitializeTargetDirectories();
            _fileOperation.SendMessage += SendLog;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            var destination = _fileOperation.FindDestinationDirectory(fileSystemEventArgs.Name, 
                _rules, _settingSection.DefaultDirectory.Path);
            _fileOperation.CreateDirectoryIfNotExist(destination);

            var fileName = string.Empty;

            if (_settingSection.OrderNumber.IsEnabled)
            {
                var counter = System.IO.Directory.GetFileSystemEntries(destination).Length + 1;
                fileName = $"{counter}. " + fileSystemEventArgs.Name;
            }

            if (_settingSection.Date.IsEnabled)
                fileName = _fileOperation.GetFileNameWithDate(fileName);

            var path = Path.Combine(destination, fileName);

            if (_fileOperation.IsFileExists(path)) return; 

            _fileOperation.CopyFile(fileSystemEventArgs.FullPath, path);
        }

        public void Dispose()
        {
            foreach (var fileSystemWatcher in _fileSystemWatchers)
            {
                fileSystemWatcher.Created -= OnFileCreated;
                fileSystemWatcher.Dispose();
            }
        }

        private void InitializeTargetDirectories()
        {
            foreach (var directory in _settingSection.Directories)
            {
                var path = (directory as Directory).Name;

                 _fileOperation.CreateDirectoryIfNotExist(path);

                var fileSystemWatcher =
                    new FileSystemWatcher(path) { EnableRaisingEvents = true };

                fileSystemWatcher.Created += OnFileCreated;
                _fileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        public event SendMessageHandler SendMessage;

        public void SendLog(string msg)
        {
            SendMessage?.Invoke(msg + $" ([{DateTime.Now.ToString(CultureInfo.CurrentCulture)}])");
        }
    }
}