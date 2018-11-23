using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace HttpModuleSamples
{
    public class HelloModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += AuthenticateRequest;
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var name = Thread.CurrentPrincipal.Identity.Name;
            app.Response.Output.WriteLine($"Hello, {name}!");
            app.CompleteRequest();
        }

        private void AuthenticateRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var user = app.Context.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                var name = app.Request.QueryString["name"] ?? "Anonymouse";

                var principal = new GenericPrincipal(
                    new GenericIdentity(name), new string[] { });
                app.Context.User = principal;
            }
        }
    }
}