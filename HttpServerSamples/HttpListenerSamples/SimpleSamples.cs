using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HttpListenerSamples
{
    [TestClass]
    public class SimpleSamples
    {
        [TestMethod]
        public void HelloSample()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://+:81/");
            listener.Start();

            do
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                if (request.Url.PathAndQuery == "/~close")
                {
                    response.Close();
                    break;
                }

                var name = request.Url.ParseQueryString()["name"];

                var answerString = string.IsNullOrEmpty(name) ?
                    "Hello, Anonymouse!" : $"Hello, {name}";

                var buffer = Encoding.UTF8.GetBytes(answerString);

                response.ContentLength64 = buffer.Length;
                response.StatusCode = (int)HttpStatusCode.OK;

                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            while (true);

            listener.Stop();

        }
    }
}
