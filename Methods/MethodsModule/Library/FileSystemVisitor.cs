using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library.EventArgs;

namespace Library
{
    public class FileSystemVisitor
    {
        public event EventHandler<ItemFindedArg<FileSystemInfo>> FileFinded;
        public event EventHandler<ItemFindedArg<FileSystemInfo>> DirectoryFinded;
        public event EventHandler<ItemFindedArg<FileSystemInfo>> FilteredFileFinded;
        public event EventHandler<ItemFindedArg<FileSystemInfo>> FilteredDirectoryFinded;
        public event EventHandler<StartArg> Start;
        public event EventHandler<FinishArg> Finish;

        private readonly DirectoryInfo _directoryInfo;
        private readonly Func<DirectoryInfo, bool> _directoryFilter;
        private readonly Func<FileInfo, bool> _fileFilter;

        private readonly List<ItemFindedArg<FileSystemInfo>> _argList;
        private List<FileSystemInfo> _fileSystemInfos;

        public FileSystemVisitor(DirectoryInfo directoryInfo, Func<DirectoryInfo, bool> directoryFilter = null,
            Func<FileInfo, bool> fileFilter = null)
        {
            _directoryInfo = directoryInfo;
            _directoryFilter = directoryFilter;
            _fileFilter = fileFilter;
            _argList = new List<ItemFindedArg<FileSystemInfo>>();
            _fileSystemInfos = new List<FileSystemInfo>();
        }

        public IEnumerable<FileSystemInfo> GenerateDirectoryTree()
        {
            Clear();

            OnStart();

            StartSearch(_directoryInfo);

            ClearExcludedItems();

            OnFinish();

            return _fileSystemInfos;
        }

        private void StartSearch(DirectoryInfo directoryInfo)
        {
            var accessControl = directoryInfo.GetAccessControl();

            if (accessControl.AreAccessRulesProtected)
                return;

            foreach (var systemInfo in directoryInfo.EnumerateFileSystemInfos())
            {
                if (IsSearchStopped())
                    return;

                switch (systemInfo)
                {
                    case FileInfo file:
                        RegisterFile(file);
                        break;
                    case DirectoryInfo subDirectory:
                       RegisterDirectory(subDirectory);
                        StartSearch(subDirectory);
                        break;
                }
            }
        }

        private void RegisterFile(FileInfo fileInfo)
        {
            OnFileFinded(fileInfo);

            if (IsFileFilterPassed(fileInfo))
            {
                OnFileFinded(fileInfo, true);
                _fileSystemInfos.Add(fileInfo);
            }
            else if (_fileFilter == null)
                _fileSystemInfos.Add(fileInfo);
        }

        private void RegisterDirectory(DirectoryInfo directoryInfo)
        {
            OnDirectoryFinded(directoryInfo);
            if (IsDirectoryFilterPassed(directoryInfo))
            {
                OnDirectoryFinded(directoryInfo, true);
                _fileSystemInfos.Add(directoryInfo);
            }
            else if (_directoryFilter == null)
                _fileSystemInfos.Add(directoryInfo);
        }

        private bool IsFileFilterPassed(FileInfo fileInfo)
        {
            return _fileFilter != null && _fileFilter(fileInfo);
        }

        private bool IsDirectoryFilterPassed(DirectoryInfo directoryInfo)
        {
            return _directoryFilter != null && _directoryFilter(directoryInfo);
        }

        private bool IsSearchStopped()
        {
            return _argList.Any(arg => arg.StopSearch);
        }

        private void ClearExcludedItems()
        {
            _fileSystemInfos = _fileSystemInfos.Where(item => !_argList.Any(
                arg => arg.IsExcluded && arg.FileSystemInfo == item)).ToList();
        }

        private void Clear()
        {
            _argList.Clear();
            _fileSystemInfos.Clear();
        }

        protected virtual void OnFileFinded(FileInfo fileInfo, bool isFiltered = false)
        {
            var arg = new ItemFindedArg<FileSystemInfo>(fileInfo);
            _argList.Add(arg);

            if (isFiltered)
                FilteredFileFinded?.Invoke(this, arg);

            else
                FileFinded?.Invoke(this, arg);
        }

        protected virtual void OnDirectoryFinded(DirectoryInfo directoryInfo, bool isFiltered = false)
        {
            var arg = new ItemFindedArg<FileSystemInfo>(directoryInfo);
            _argList.Add(arg);

            if (isFiltered)
                FilteredDirectoryFinded?.Invoke(this, arg);

            else
                DirectoryFinded?.Invoke(this, arg);
        }

        protected virtual void OnStart()
        {
            Start?.Invoke(this, new StartArg($"[{DateTime.Now}]: Start of search."));
        }

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, new FinishArg($"[{DateTime.Now}]: Search was finished."));
        }
    }
}
