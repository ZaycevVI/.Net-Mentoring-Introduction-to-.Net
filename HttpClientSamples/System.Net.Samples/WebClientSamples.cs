using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.Samples
{
    [TestClass]
    public class WebClientSamples
    {
        [TestMethod]
        public void DownloadFile()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(
                    "https://epam-my.sharepoint.com/personal/mihail_romanov_epam_com/_layouts/15/guestaccess.aspx?docid=19c99c6713d8b46579538d099f088b7e6&authkey=AQjj9ONV8IVQlSuL3onj5ts",
                    "e3smeta1.png");
            }
        }
    }
}
