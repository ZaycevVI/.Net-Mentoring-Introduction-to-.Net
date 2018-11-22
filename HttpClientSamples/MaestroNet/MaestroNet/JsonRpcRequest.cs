using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaestroNet
{
    class JsonRpcRequest<T> where T : new()
    {
        [JsonProperty("jsonrpc")]
        protected readonly string JsonRpcVersion;

        [JsonProperty("id")]
        protected int Id { get; set; }

        [JsonProperty("method")]
        internal string Method { get; set; }

        [JsonProperty("params", NullValueHandling=NullValueHandling.Include)]
        internal T Params { get; set; }

        public JsonRpcRequest()
        {
            JsonRpcVersion = "2.0";
            Id = 1;
            Method = "";

            Params = new T();
        }
    }
}
