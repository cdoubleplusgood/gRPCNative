using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using AspNetGrpc.Greeter;

namespace AspNetGrpc.GreeterClient48
{
    internal class Program
    {
        static async Task Main()
        {
            const int httpsPort = 7142;
            //const int httpPort = 5113;
            var httpHandler = new GrpcWebHandler(new HttpClientHandler());
            var channelOptions = new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            };
            using (var channel = GrpcChannel.ForAddress($"https://localhost:{httpsPort}", channelOptions))
            {
                await UnaryAsync(channel, TimeSpan.FromSeconds(3.0));
                //await ServerStreamAsync(channel, TimeSpan.FromSeconds(10.0));
                //await ClientStreamAsync(channel);
                //await DuplexStreamAsync(channel);

                //await ServerStreamWithCancellationAsync(channel);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static async Task UnaryAsync(GrpcChannel channel, TimeSpan deadline)
        {
            var client = new Greeter.Greeter.GreeterClient(channel);
            var request = new HelloRequest
            {
                Name = "Peter Parker"
            };
            Console.WriteLine("=== SayHello ===");
            Console.WriteLine($"Request: {request.Name}");
            try
            {
                var reply = await client.SayHelloAsync(request, null, DateTime.UtcNow + deadline);
                Console.WriteLine($"Reply: {reply.Message}");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Timeout");
            }
        }
    }
}
