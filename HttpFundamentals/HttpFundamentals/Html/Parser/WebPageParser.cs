using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace HttpFundamentals.Html.Parser
{
    public class WebPageParser : IWebPageParser
    {
        private readonly HtmlParser _htmlParser;

        public WebPageParser()
        {
            _htmlParser = new HtmlParser();
        }

        public IHtmlDocument Parse(string html)
        {
            return _htmlParser.Parse(html);
        }
    }
}