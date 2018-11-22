using MaestroNet.Projects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace MaestroNet
{
    class MaestroClient : IMaestroClient
    {
        string Uri { get; set; }
        string Id { get; set; }
        string Token { get; set; }

        public MaestroClient(string uri, string id, string token)
        {
            Uri = uri;
            Id = id;
            Token = token;
        }

        public void Dispose()
        {
        }

        TResult ProcessRequest<TParams, TResult>(TParams param, string methodName)
            where TParams : new()
            where TResult : new()
        {
            var request = new JsonRpcRequest<TParams>();
            request.Params = param;
            request.Method = methodName;
                      
            
            var requestString = JsonConvert.SerializeObject(request);

            using (var handler = new MaestroHttpHandler(Id, Token))
            using (var httpClient = new HttpClient(handler))
            {
                var result = httpClient.PostAsync(Uri, new StringContent(requestString, Encoding.UTF8, "application/json")).Result;

                if (result.IsSuccessStatusCode)
                {
                    var responseString = result.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<JsonRpcResponse<TResult>>(responseString);
                    return response.Result;
                }
                else
                    throw new Exception("Proccessing error");
            }
        }


        public IEnumerable<Projects.Project> DescribeProjects()
        {
            var param = new DescribeProjectsRequestParams();
            var result = ProcessRequest<DescribeProjectsRequestParams, DescribeProjectsResponseResult>(param, "describeProjects");

            return result.Projects;
        }
    }
}
