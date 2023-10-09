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
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            var channel = new Channel("localhost", 50051, ChannelCredentials.Insecure);
            try
            {
                var greetingService = channel.CreateGrpcService<IGreetingService>();

                var greetingRequest = new GreetingRequest
                {
                    Greeting =
                    {
                        FirstName = "Peter",
                        LastName = "Parker"
                    }
                };
                Console.WriteLine("Greet");
                Console.WriteLine("GreetRequest: " +
                                  $"{greetingRequest.Greeting.FirstName} " +
                                  $"{greetingRequest.Greeting.LastName}");
                var response = await greetingService.Greet(greetingRequest);
                Console.WriteLine($"GreetResponse: {response.Response}");

                var greetOftenRequest = new GreetOftenRequest
                {
                    Greeting =
                    {
                        FirstName = "Peter",
                        LastName = "Parker"
                    },
                    Count = 5
                };
                Console.WriteLine("GreetOften");
                Console.WriteLine("GreetOftenRequest:" +
                                  $"{greetOftenRequest.Greeting.FirstName} " +
                                  $"{greetOftenRequest.Greeting.LastName} " +
                                  $"{greetOftenRequest.Count}");
                var streamResponse = greetingService.GreetOften(greetOftenRequest);
                await foreach (var response2 in streamResponse)
                {
                    Console.WriteLine($"GreetResponse: {response2.Response}");
                }
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
