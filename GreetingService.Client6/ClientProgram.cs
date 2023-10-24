using GreetingService.Common;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace GreetingService.Client6
{
    internal class ClientProgram
    {
        static async Task Main()
        {
            using (var channel = GrpcChannel.ForAddress("https://localhost:50051"))
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
                var streamResponse = client.GreetOften(greetOftenRequest);
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
                    await client.GreetAll(ToAsyncEnumerable(greetingRequests, async g =>
                    {
                        Console.WriteLine("GreetRequest: " +
                                          $"{g.Greeting.FirstName} " +
                                          $"{g.Greeting.LastName}");
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }));
                Console.WriteLine($"GreetResponse: {greetAllResponse.Response}");
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
