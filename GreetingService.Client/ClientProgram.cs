using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Common;
using Grpc.Core;
using ProtoBuf.Grpc.Client;

namespace GreetingService.Client
{
    internal class ClientProgram
    {
        static void Main()
        {
            TestGreetingService().Wait();
        }

        static async Task TestGreetingService()
        {
            var request = new GreetingRequest
            {
                Greeting =
                {
                    FirstName = "Peter",
                    LastName = "Parker"
                }
            };
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            var channel = new Channel("localhost", 50051, ChannelCredentials.Insecure);
            try
            {
                var greetingService = channel.CreateGrpcService<IGreetingService>();
                var response = await greetingService.Greet(request);
            }
            finally
            {
                await channel.ShutdownAsync();
            }
        }
    }
}
