using System;
using System.IO;
using System.Net;
using AngleSharp;
using HttpFundamentals.Html.Converter;

namespace HttpFundamentals.Html.FileSystem
{
    public class FileWrapper : IFileWrapper
    {
        private const string DefaultHtmlFileName = "Index.html";
        private readonly string _basePath;

        public FileWrapper(string rootUrl)
        {
            _basePath = new Url(rootUrl).Host;
        }

        public void CreateHtml(string url, string content)
        {
            var fullPath = url.ToPhysicalPath() + $"\\{DefaultHtmlFileName}";

            if (!fullPath.StartsWith(_basePath))
                fullPath = $"{_basePath}\\{fullPath}";

            if (!File.Exists(fullPath))
                File.WriteAllText(fullPath, content);
        }

        public void CreateFile(string urlPath, string rootUrl)
        {
            urlPath = urlPath.Replace("about://", string.Empty);
            var url = new Url(urlPath);
            var filePath = PathConverter.ToPhysicalPath(urlPath, rootUrl);

            if (url.IsRelative)
                url = new Url(new Url(rootUrl), urlPath);

            if (!filePath.StartsWith(_basePath))
                filePath = $"{_basePath}\\{filePath}";

            using (var client = new WebClient())
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    // ReSharper disable once AssignNullToNotNullAttribute
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                try
                {
                    client.DownloadFile(url, filePath);
                }
                catch (WebException e)
                {
                    Console.WriteLine($"{DateTime.Now}: Failed to download file from url \"{url}\", to path \"{filePath}\"."
                        + $"{Environment.NewLine}[Error]: {e.Message}");
                }
            }
        }
    }
}