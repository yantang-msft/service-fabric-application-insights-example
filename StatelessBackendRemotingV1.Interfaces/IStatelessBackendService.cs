using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessBackendRemotingV1.Interfaces
{
    public interface IStatelessBackendService : IService
    {
        Task<long> GetCountAsync();
    }
}
