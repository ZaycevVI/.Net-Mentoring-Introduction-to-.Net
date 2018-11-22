using MaestroNet.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaestroNet
{
    public interface IMaestroClient : IDisposable
    {
        IEnumerable<Project> DescribeProjects();
    }
}
