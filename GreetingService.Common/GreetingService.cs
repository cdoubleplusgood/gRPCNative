using ProtoBuf.Grpc.Configuration;
using System.Threading.Tasks;

namespace GreetingService.Common
{
    [Service("GreetingService")]
    public interface IGreetingService
    {
        Task<GreetingResponse> Greet(GreetingRequest request);
    }
}
