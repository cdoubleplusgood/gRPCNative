using System.Collections.Generic;
using ProtoBuf.Grpc.Configuration;
using System.Threading.Tasks;

namespace GreetingService.Common
{
    [Service]
    public interface IGreetingService
    {
        Task<GreetingResponse> Greet(GreetingRequest request);

        IAsyncEnumerable<GreetingResponse> GreetOften(GreetOftenRequest request);

        Task<GreetingResponse> GreetAll(IAsyncEnumerable<GreetingRequest> requests);
    }
}
