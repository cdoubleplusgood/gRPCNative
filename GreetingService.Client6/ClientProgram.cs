using GreetingService.Common;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace GreetingService.Client6
{
    internal class ClientProgram
    {
        static async Task Main()
        {
            using (var channel = GrpcChannel.ForAddress("http://localhost:5151"))
            {
                var client = channel.CreateGrpcService<IGreetingService>();
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
            }

            Console.WriteLine("The client will stop now.");
            Console.ReadKey();
        }
    }
}
