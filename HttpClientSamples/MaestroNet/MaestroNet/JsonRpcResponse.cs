using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaestroNet
{
    class JsonRpcResponse<T> where T : new()
    {
        [JsonProperty("jsonrpc")]
        protected string JsonRpcVersion;

        [JsonProperty("id")]
        protected int Id { get; set; }
               
        [JsonProperty("result")]
        internal T Result { get; set; }

        public JsonRpcResponse()
        {
        }
    }
}
