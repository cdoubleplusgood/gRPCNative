using ProtoBuf.Grpc.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Common
{
    [Service("GreetingService")]
    public interface IGreetingService
    {
        Task<GreetingResponse> Greet(GreetingRequest request);
    }
}
