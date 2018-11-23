using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom.Html;
using HttpFundamentals.Enum;
using HttpFundamentals.Html.Converter;
using HttpFundamentals.Html.FileSystem;
using HttpFundamentals.Html.Parser;
using HttpFundamentals.Html.Reader;
using HttpFundamentals.Validation;
using Url = AngleSharp.Url;

namespace HttpFundamentals.Html.Loader
{
    public class WebPageLoader : IWebPageLoader
    {
        private readonly string _rootUrl;
        private readonly IWebPageParser _parser;
        private readonly IWebPageReader _reader;
        private readonly IDirectoryWrapper _directoryWrapper; 
        private readonly IFileWrapper _fileWrapper;
        private readonly List<string> _availableExtensions;
        private const int MaxSteps = 3;
        private readonly bool _isVerbose;
        private readonly DomainLimit _domainLimit;

        public WebPageLoader(string rootUrl, List<string> extensions = null, 
            bool isVerbose = true, DomainLimit domainLimit = DomainLimit.WithoutLimits)
        {
            _rootUrl = rootUrl;
            _availableExtensions = extensions;
            _parser = new WebPageParser();
            _reader = new WebPageReader();
            _directoryWrapper = new DirectoryWrapper(rootUrl);
            _fileWrapper = new FileWrapper(rootUrl);
            _isVerbose = isVerbose;
            _domainLimit = domainLimit;
        }

        public void Load(uint steps)
        {
            CreatePage(_rootUrl, steps > MaxSteps ? MaxSteps : steps, new List<string>());
        }

        private void CreatePage(string url, uint maxStep, List<string> analyzedUrls, uint stepCount = 0)
        {
            if(stepCount > maxStep)
                return;

            if(!DomainValidator.IsValid(_domainLimit, url, _rootUrl))
                return;

            if(_isVerbose)
                Console.WriteLine($"Analyzing url: {url}");


            _directoryWrapper.Create(url);
            var html = _reader.Read(url);
            _fileWrapper.CreateHtml(url, html);

            var doc = _parser.Parse(html);

            CopyFiles(doc, url);
            analyzedUrls.Add(url);
            var links = GetLinks(doc, url);

            foreach (var link in links)
            {
                var linkUrl = PathConverter.ToUrl(url, link.Href);

                if(analyzedUrls.Contains(linkUrl))
                    continue;

                CreatePage(linkUrl,maxStep, analyzedUrls, stepCount + 1);
            }
        }

        #region CopyFiles

        private void CopyFiles(IHtmlDocument document, string url)
        {
            CopyImages(document, url);
            CopyCss(document, url);
            CopyScripts(document, url);
        }

        private void CopyImages(IHtmlDocument document, string url)
        {
            var images = document.Images;
            var imgSources = images.Select(i => i.Source)
                .Where(s => !string.IsNullOrEmpty(s) && !s.EndsWith("/") 
                && IsMappedExtension(s));
            foreach (var imgSource in imgSources)
                _fileWrapper.CreateFile(imgSource, url);
        }

        private void CopyScripts(IHtmlDocument document, string url)
        {
            var scripts = document.Scripts;
            var scriptsSource = scripts.Select(s => s.Source)
                .Where(s => !string.IsNullOrEmpty(s) 
                && !s.EndsWith("/") && IsMappedExtension(s));

            foreach (var scriptSource in scriptsSource)
                _fileWrapper.CreateFile(scriptSource, url);
        }

        private void CopyCss(IHtmlDocument document, string url)
        {
            var stylesHref = document.QuerySelectorAll("link[href]")
                .OfType<IHtmlLinkElement>().Select(s => s.Href)
                .Where(s => !string.IsNullOrEmpty(s) 
                && !s.EndsWith("/") && IsMappedExtension(s));

            foreach (var styleSource in stylesHref)
                _fileWrapper.CreateFile(styleSource, url);
        }

        #endregion

        private IEnumerable<IHtmlAnchorElement> GetLinks(IHtmlDocument document, string url)
        {
            return document
                .Links.OfType<IHtmlAnchorElement>()
                .Where(l => !string.IsNullOrEmpty(l.Href) 
                && l.Href != "about:///"
                && l.Href != "about://"
                && !l.Href.EndsWith(new Url(url).Path));
        }

        private bool IsMappedExtension(string path)
        {
            return _availableExtensions == null || _availableExtensions.Any(path.Contains);
        }
    }
}