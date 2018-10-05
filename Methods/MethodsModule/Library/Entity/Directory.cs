using System.Collections.Generic;
using System.IO;

namespace Library.Entity
{
    public class Directory
    {
        public List<FileInfo> Files { get; }
        public List<DirectoryInfo> Directories { get; }

        public Directory()
        {
            Files = new List<FileInfo>();
            Directories = new List<DirectoryInfo>();
        }
    }
}
