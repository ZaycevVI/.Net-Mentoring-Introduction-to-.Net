using System.IO;
using AngleSharp;
using HttpFundamentals.Html.Converter;

namespace HttpFundamentals.Html.FileSystem
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        private readonly string _basePath;

        public DirectoryWrapper(string rootUrl)
        {
            _basePath = new Url(rootUrl).Host;
        }

        public void Create(string urlPath)
        {
            var path = urlPath.ToPhysicalPath();

            if (!path.StartsWith(_basePath))
                path = $"{_basePath}\\{path}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}