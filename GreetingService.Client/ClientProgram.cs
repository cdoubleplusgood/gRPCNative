using System;
using System.Collections.Generic;
using System.IO;
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
            //GrpcClientFactory.AllowUnencryptedHttp2 = true;
            var clientCert = File.ReadAllText("ssl/localhost2-chain.crt");
            var clientKey = File.ReadAllText("ssl/localhost2.key");
            var caCert = File.ReadAllText("ssl/dev-root-ca.crt");
            var channelCredentials = new SslCredentials(caCert, new KeyCertificatePair(clientCert, clientKey));

            var channel = new Channel("localhost", 50051, channelCredentials);
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

                var greetingRequests = new List<GreetingRequest>
                {
                    new GreetingRequest { Greeting = { FirstName = "Peter", LastName = "Parker" } },
                    new GreetingRequest { Greeting = { FirstName = "Tony", LastName = "Stark" } },
                    new GreetingRequest { Greeting = { FirstName = "Steve", LastName = "Rogers" } }
                };
                Console.WriteLine("GreetAll");
                var greetAllResponse =
                    await greetingService.GreetAll(ToAsyncEnumerable(greetingRequests, async g =>
                    {
                        Console.WriteLine("GreetRequest: " +
                                          $"{g.Greeting.FirstName} " +
                                          $"{g.Greeting.LastName}");
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }));
                Console.WriteLine($"GreetResponse: {greetAllResponse.Response}");
            }
            finally
            {
                await channel.ShutdownAsync();
            }

            Console.WriteLine("The client will stop now.");
            Console.ReadKey();
        }

        static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> items, Func<T, Task> action)
        {
            foreach (var item in items)
            {
                // This is mainly to avoid the warning about the seemingly unnecessary "async",
                // but it is required!
                await action(item);
                yield return item;
            }
        }
    }
}
