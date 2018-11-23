using AngleSharp;
using HttpFundamentals.Enum;
using HttpFundamentals.Html.Loader;

namespace HttpFundamentals
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebPageLoader loader = new WebPageLoader("https://html-agility-pack.net/from-web", 
                domainLimit: DomainLimit.NotHigherThanThisDomain);
            loader.Load(3);
        }
    }
}
