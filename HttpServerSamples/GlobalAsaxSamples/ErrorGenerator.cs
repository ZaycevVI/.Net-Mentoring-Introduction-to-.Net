using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalAsaxSamples
{
    public class ErrorGenerator : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            throw new Exception("Some error");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}