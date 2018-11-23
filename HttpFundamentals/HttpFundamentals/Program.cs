using HttpFundamentals.Html.Loader;

namespace HttpFundamentals
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebPageLoader loader = new WebPageLoader("https://html-agility-pack.net/from-web");
            loader.Load(3);
        }
    }
}
