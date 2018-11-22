using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Text;
using System.IO;

namespace System.Net.Samples
{
    [TestClass]
    public class HttpWebRequestSamples
    {
        private const string HashStringDelim = ":";
        private const string ApiVersion = "4.1.31";

        string token = "258e8bfa-d1e7-4aeb-bd52-f38f66188267";
        string id = "mihail_romanov@epam.com";



        [TestMethod]
        public void GetMaestroProjects()
        {
            var request = WebRequest.CreateHttp("https://orchestration.epam.com/maestro2/api/rpc");

            var maestroDate = GetMaestroDate(DateTime.Now);
            var content = "{\"jsonrpc\":\"2.0\",\"id\":1,\"method\":\"describeProjects\",\"params\":{}}";
            var autorizationString = GetMaestroAuthorizationString(id, token, content, maestroDate);

            request.Method = "POST";

            request.Headers.Add("Maestro-access-id", id);
            request.Headers.Add("Maestro-api-version", ApiVersion);
            request.Headers.Add("Maestro-date", maestroDate);
            request.Headers.Add("Maestro-authorization", autorizationString);
            request.ContentType = "application/json";

            var requestBody = Encoding.UTF8.GetBytes(content);

            request.GetRequestStream().Write(requestBody, 0, requestBody.Length);

            var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());

            var answer = reader.ReadToEnd();

            Console.WriteLine(answer);
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

        private string GetMaestroDate(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("R", CultureInfo.GetCultureInfo("en-US"));
        }

        private string GetHashString(string id, string content, string date)
        {
            var stringBuilder = new StringBuilder(256);
            stringBuilder
                .Append("POST")
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
