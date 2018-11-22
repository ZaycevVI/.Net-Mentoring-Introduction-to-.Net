using System;
using System.Linq;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using HttpFundamentals.Html.Parser;
using HttpFundamentals.Http.Reader;

namespace HttpFundamentals
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new WebPageReader();
            var html = reader.Read("https://stackoverflow.com/");
            Console.WriteLine(html);
            var parser = new WebPageParser();
            var dom = parser.Parse(html);
            var @as = dom.Links;
            var a = @as.Last() as IHtmlAnchorElement;
            a.Href = "aaa";
            Console.WriteLine(dom.ToHtml());
        }
    }
}
