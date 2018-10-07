using System;
using System.IO;

namespace Library.EventArgs
{
    public class ItemFindedArg<T> : FileSystemVisitorBaseArg
        where T : FileSystemInfo
    {
        public ItemFindedArg(FileSystemInfo fileSystemInfo)
        {
            FileSystemInfo = fileSystemInfo;
            var type = fileSystemInfo is FileInfo ? "File" : "Directory";
            Message = $"[{DateTime.Now}] [{type}]: {FileSystemInfo.FullName}";
        }

        public FileSystemInfo FileSystemInfo { get; }

        public bool StopSearch { get; set; }

        public bool IsExcluded { get; set; }
        public override string Message { get; }
    }
}
