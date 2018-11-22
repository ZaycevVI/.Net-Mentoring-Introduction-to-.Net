using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaestroNet
{
    public class MaestroClientFactory
    {
        public IMaestroClient CreateClient(string uri, string id, string token)
        {
            return new MaestroClient(uri, id, token);
        }
    }
}
