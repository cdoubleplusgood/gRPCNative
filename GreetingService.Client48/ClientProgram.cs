using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using GreetingService.Common;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using ProtoBuf.Grpc.Client;

namespace GreetingService.Client48
{
    internal class ClientProgram
    {
        static async Task Main()
        {
            const int httpsPort = 50051;
            var httpHandler = new GrpcWebHandler(new HttpClientHandler());
            var channelOptions = new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            };
            using (var channel =
                   GrpcChannel.ForAddress($"https://localhost:{httpsPort}", channelOptions))
            {
                var client = channel.CreateGrpcService<IGreetingService>();

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
                var response = await client.Greet(greetingRequest);
                Console.WriteLine($"GreetResponse: {response.Response}");

                //var greetOftenRequest = new GreetOftenRequest
                //{
                //    Greeting =
                //    {
                //        FirstName = "Peter",
                //        LastName = "Parker"
                //    },
                //    Count = 5
                //};
                //Console.WriteLine("GreetOften");
                //Console.WriteLine("GreetOftenRequest:" +
                //                  $"{greetOftenRequest.Greeting.FirstName} " +
                //                  $"{greetOftenRequest.Greeting.LastName} " +
                //                  $"{greetOftenRequest.Count}");
                //var streamResponse = client.GreetOften(greetOftenRequest);
                //await foreach (var response2 in streamResponse)
                //{
                //    Console.WriteLine($"GreetResponse: {response2.Response}");
                //}

                //var greetingRequests = new List<GreetingRequest>
                //{
                //    new GreetingRequest { Greeting = { FirstName = "Peter", LastName = "Parker" } },
                //    new GreetingRequest { Greeting = { FirstName = "Tony", LastName = "Stark" } },
                //    new GreetingRequest { Greeting = { FirstName = "Steve", LastName = "Rogers" } }
                //};
                //Console.WriteLine("GreetAll");
                //var greetAllResponse =
                //    await client.GreetAll(ToAsyncEnumerable(greetingRequests, async g =>
                //    {
                //        Console.WriteLine("GreetRequest: " +
                //                          $"{g.Greeting.FirstName} " +
                //                          $"{g.Greeting.LastName}");
                //        await Task.Delay(TimeSpan.FromSeconds(1));
                //    }));
                //Console.WriteLine($"GreetResponse: {greetAllResponse.Response}");
            }

            Console.WriteLine("The client will stop now.");
            Console.ReadKey();
        }
     }
}