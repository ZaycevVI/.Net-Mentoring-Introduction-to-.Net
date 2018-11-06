using System;
using FileLibrary.Logger;

namespace FileLibrary.File
{
    public interface IFileNotifier : ILogNotifier, IDisposable
    {
    }
}