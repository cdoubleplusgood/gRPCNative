using System;
using Grpc.Net.Client;

namespace AspNetGrpc.GreeterClient48
{
    internal class Program
    {
        static void Main()
        {
            const int httpsPort = 7142;
            //const int httpPort = 5290;
            using (var channel = GrpcChannel.ForAddress($"https://localhost:{httpsPort}"))
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
