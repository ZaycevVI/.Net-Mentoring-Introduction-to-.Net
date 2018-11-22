using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace GlobalAsaxSamples
{
    public class Global : System.Web.HttpApplication
    {
        private static string LogFilePath;

        protected void Application_Start(object sender, EventArgs e)
        {
            LogFilePath = Path.Combine(Server.MapPath("/"), "log.txt");
            File.AppendAllText(LogFilePath, $"Application started {DateTime.Now}\n");
        }
                        
        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();

            File.AppendAllText(LogFilePath, $"Error {error.Message}\n");

            Server.ClearError();
        }
                
        protected void Application_End(object sender, EventArgs e)
        {
            File.AppendAllText(LogFilePath, $"Application stopped {DateTime.Now}\n");
        }
    }
}