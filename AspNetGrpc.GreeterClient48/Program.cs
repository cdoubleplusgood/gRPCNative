using System;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace AspNetGrpc.GreeterClient48
{
    internal class Program
    {
        static void Main()
        {
            const int httpsPort = 7142;
            //const int httpPort = 5290;
            var httpHandler = new GrpcWebHandler(new HttpClientHandler());
            var channelOptions = new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            };
            using (var channel = GrpcChannel.ForAddress($"https://localhost:{httpsPort}", channelOptions))
            {
            }

            //await UnaryAsync(channel, TimeSpan.FromSeconds(3.0));
            //await ServerStreamAsync(channel, TimeSpan.FromSeconds(10.0));
            //await ClientStreamAsync(channel);
            //await DuplexStreamAsync(channel);

            //await ServerStreamWithCancellationAsync(channel);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
