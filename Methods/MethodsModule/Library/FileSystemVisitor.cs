using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library
{
    public class FileSystemVisitor : IEnumerable<FileSystemInfo>
    {
        private string _path;
        public string Path
        {
            get => _path ?? string.Empty;
            set => _path = value;
        }

        private DirectoryInfo _directoryInfo;

        private DirectoryInfo _directoryInfoCursor;

        public FileSystemVisitor(string path)
        {
            Path = path;
            _directoryInfoCursor = _directoryInfo =
                new DirectoryInfo(Path);
        }

        public IEnumerator<FileSystemInfo> GetEnumerator()
        {
            if (_directoryInfoCursor == null || !_directoryInfoCursor.Exists)
                yield break;

            var files = _directoryInfoCursor.GetFiles();
            var directories = _directoryInfoCursor.GetDirectories();


            foreach (var directory in directories)
            {
                yield return directory;
            }

            foreach (var file in files)
            {
                yield return file;
            }

            GetEnumerator();
        }

        private IEnumerator<FileSystemInfo> GetFileSystemInfo(DirectoryInfo directoryInfo)
        {
            var files = _directoryInfoCursor.GetFiles();
            var subDirectories = _directoryInfoCursor.GetDirectories();

            foreach (var directory in subDirectories)
            {
                yield return directory;
            }

            foreach (var file in files)
            {
                yield return file;
            }

            foreach (var subDirectory in subDirectories)
            {
                foreach (var VARIABLE in GetFileSystemInfo(subDirectory))
                {
                    
                }

                
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
