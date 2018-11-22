using AngleSharp.Dom.Html;

namespace HttpFundamentals.Html.Parser
{
    public interface IWebPageParser
    {
        IHtmlDocument Parse(string html);
    }
}