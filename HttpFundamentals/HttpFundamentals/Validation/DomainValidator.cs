using System;
using HttpFundamentals.Enum;

namespace HttpFundamentals.Validation
{
    public class DomainValidator
    {
        public static bool IsValid(DomainLimit domainLimit, string urlToCompare, string rootUrl)
        {
            var urlRoot = new Uri(rootUrl);
            var url = new Uri(urlToCompare);

            switch (domainLimit)
            {
                case DomainLimit.WithoutLimits:
                    return true;
                case DomainLimit.NotHigherThanThisDomain:
                    return Uri.Compare(urlRoot, url, UriComponents.Host | UriComponents.PathAndQuery,
                               UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) >= 0;
                case DomainLimit.OnlyThisDomain:
                    return urlRoot.Host == url.Host;
            }

            return false;
        }
    }
}
