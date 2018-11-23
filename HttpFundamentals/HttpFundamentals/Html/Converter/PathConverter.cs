using AngleSharp;
using HttpFundamentals.Helper;

namespace HttpFundamentals.Html.Converter
{
    public static class PathConverter
    {
        /// <summary>
        /// 3 cases:
        /// 1) www.w3w.com/form && path1/path2 --> www.w3w.com\path1\path2
        /// 2) www.w3w.com/form && www.github.com/branch/master --> www.w3w.com\www.github.com\branch\master
        /// 3) www.w3w.com/form && www.w3w.com/branch/master --> www.w3w.com\branch\master
        /// </summary>
        public static string ToPhysicalPath(string attributeUrl, string rootUrl)
        {
            var urlAttribute = new Url(attributeUrl);
            var urlRoot = new Url(rootUrl);
            var physicalPath = string.Empty;

            if (urlAttribute.IsRelative)
                physicalPath = $"{urlRoot.Host}/{urlAttribute.Path}";

            else if (urlAttribute.IsAbsolute)
            {
                physicalPath = urlAttribute.Host != urlRoot.Host
                    ? $"{urlRoot.Host}\\{ToPhysicalPath(attributeUrl)}"
                    : $"{ToPhysicalPath(attributeUrl)}";
            }

            return physicalPath.Trim('\\').ReplaceSlashes();
        }


        /// <summary>
        /// From: test.com/test, To: test.com\test
        /// </summary>
        public static string ToPhysicalPath(this string url)
        {
            var urlPath = new Url(url);

            return (urlPath.Host + "\\" + urlPath.Path).Trim('\\').ReplaceSlashes();
        }

        public static string ToUrl(string root, string relative)
        {
            var rootUrl = new Url(root);
            relative = relative.Replace("about://", string.Empty);
            var relativeUrl = new Url(relative);

            return relativeUrl.IsRelative ? $"{rootUrl.Origin}/{relativeUrl}" : relative;
        }

    }
}