using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaestroNet
{
    class MaestroHttpHandler : MessageProcessingHandler
    {
        private const string HashStringDelim = ":";
        private const string ApiVersion = "4.1.31";

        string Id { get; set; }
        string Token { get; set; }

        public MaestroHttpHandler(string id, string token)
        {
            Id = id;
            Token = token;
            InnerHandler = new HttpClientHandler();
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var maestroDate = GetMaestroDate(DateTime.Now);
            var content = request.Content.ReadAsStringAsync().Result;
            var autorizationString = GetMaestroAuthorizationString(Id, Token, content, maestroDate);

            request.Headers.Add("Maestro-access-id", Id);
            request.Headers.Add("Maestro-api-version", ApiVersion);
            request.Headers.Add("Maestro-date", maestroDate);
            request.Headers.Add("Maestro-authorization", autorizationString);
            request.Content.Headers.ContentType.CharSet = "";
            
            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response;
        }

        private string GetMaestroDate(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("R", CultureInfo.GetCultureInfo("en-US"));
        }

        private string GetMaestroAuthorizationString(string id, string token, string content, string date)
        {
            var encoding = Encoding.UTF8;
            string hashString = GetHashString(id, content, date);

            var key = encoding.GetBytes(string.Concat(token, date));
            var mac = new System.Security.Cryptography.HMACSHA256(key);

            var data = encoding.GetBytes(hashString);
            var hash = mac.ComputeHash(data);

            return Convert.ToBase64String(hash);
        }

        private string GetHashString(string id, string content, string date)
        {
            var stringBuilder = new StringBuilder(256);
            stringBuilder
                .Append(HttpMethod.Post)
                .Append(HashStringDelim)
                .Append(id)
                .Append(HashStringDelim)
                .Append(date)
                .Append(HashStringDelim)
                .Append(content);

            return stringBuilder.ToString();
        }


    }
}
