using System;
using System.Threading.Tasks;
using GreetingService.Common;
using Grpc.Core;
using ProtoBuf.Grpc.Client;

namespace GreetingService.Client
{
    internal class ClientProgram
    {
        static async Task Main()
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
                Console.WriteLine($"GreetRequest: {request.Greeting.FirstName} {request.Greeting.LastName}");
                var response = await greetingService.Greet(request);
                Console.WriteLine($"GreetResponse: {response.Response}");
            }
            finally
            {
                await channel.ShutdownAsync();
            }

            Console.WriteLine("The client will stop now.");
            Console.ReadKey();
        }
    }
}
