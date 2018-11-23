using System;
using System.Net.Http;

namespace HttpFundamentals.Html.Reader
{
    public class WebPageReader : IWebPageReader
    {
        public string Read(string url)
        {
            if(string.IsNullOrEmpty(url))
                throw new ArgumentNullException(url, nameof(url));

            string content;

            using (var httpClient = new HttpClient())
            {
                content = httpClient.GetStringAsync(url).Result;
            }

            return content;
        }
    }
}
