using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpHandlerSamples
{
    public class HelloHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.Url.PathAndQuery == "/~close")
                return;

            var name = request.QueryString["name"];

            var answerString = string.IsNullOrEmpty(name) ?
                    "Hello, Anonymouse!" : $"Hello, {name}";

            response.Output.WriteLine(answerString);
        }
    }
}