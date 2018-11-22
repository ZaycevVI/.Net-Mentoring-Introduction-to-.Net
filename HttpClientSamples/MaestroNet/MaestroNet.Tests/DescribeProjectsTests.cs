using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;

namespace MaestroNet.Tests
{
    [TestClass]
    public class DescribeProjectsTests
    {
        string token = "258e8bfa-d1e7-4aeb-bd52-f38f66188267";
        string id = "mihail_romanov@epam.com";
        string uri = "https://orchestration.epam.com/maestro2/api/rpc";

        MaestroClientFactory Factory = new MaestroClientFactory();

        [TestMethod]
        public void GetProjects()
        {           
            using (var client = Factory.CreateClient(uri, id, token))
            {
                var result = client.DescribeProjects();
                result.ToList().ForEach(p => Console.WriteLine($"{p.Name}"));
            }
        }
    }
}
