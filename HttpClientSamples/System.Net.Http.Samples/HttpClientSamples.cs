using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace System.Net.Http.Samples
{
    [TestClass]
    public class HttpClientSamples
    {
        string url = "https://epam-my.sharepoint.com/personal/mihail_romanov_epam_com/_layouts/15/guestaccess.aspx?docid=19c99c6713d8b46579538d099f088b7e6&authkey=AQjj9ONV8IVQlSuL3onj5ts";

        [TestMethod]
        public void ReadFile()
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                var file = new FileStream("image.jpg", FileMode.Create);
                client.GetAsync(url).Result
                    .Content.ReadAsStreamAsync().Result
                            .CopyTo(file);
            }
        }
    }
}
