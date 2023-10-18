using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using AspNetGrpc.Greeter;


namespace AspNetGrpc.GreeterClient48
{
    internal class ClientProgram
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
                await ServerStreamAsync(channel, TimeSpan.FromSeconds(10.0));
                await ClientStreamAsync(channel);
                //await DuplexStreamAsync(channel);

                await ServerStreamWithCancellationAsync(channel);
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

        static async Task ServerStreamAsync(GrpcChannel channel, TimeSpan deadline)
        {
            var client = new Greeter.Greeter.GreeterClient(channel);
            var request = new HelloRequest
            {
                Name = "Peter Parker"
            };
            Console.WriteLine("=== SayHelloAgain ===");
            Console.WriteLine($"Request: {request.Name}");
            try
            {
                var serverCall = client.SayHelloAgain(request, null, DateTime.UtcNow + deadline);
                var serverStream = serverCall.ResponseStream;
                while (await serverStream.MoveNext())
                {
                    var reply = serverStream.Current;
                    Console.WriteLine($"Reply: {reply.Message}");
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine(e);
            }
        }

        static async Task ClientStreamAsync(GrpcChannel channel)
        {
            var client = new Greeter.Greeter.GreeterClient(channel);
            string[] names =
            {
                "Peter Parker",
                "Tony Stark",
                "Steve Rogers",
                "Bruce Banner"
            };

            Console.WriteLine("=== SayHelloToAll ===");
            var clientStreamingCall = client.SayHelloToAll();
            var clientStream = clientStreamingCall.RequestStream;
            foreach (var name in names)
            {
                await Task.Delay(250);
                Console.WriteLine($"Request: {name}");
                await clientStream.WriteAsync(new HelloRequest { Name = name });
            }
            await clientStream.CompleteAsync();
            var reply = await clientStreamingCall;
            Console.WriteLine($"Reply: {reply.Message}");
        }

        //static async Task DuplexStreamAsync(GrpcChannel channel)
        //{
        //    var client = new Greeter.Greeter.GreeterClient(channel);
        //    string[] names =
        //    {
        //        "Peter Parker",
        //        "Tony Stark",
        //        "Steve Rogers",
        //        "Bruce Banner"
        //    };

        //    Console.WriteLine("=== SayHelloToEveryone ===");
        //    var duplexStreamingCall = client.SayHelloToEveryone();
        //    var clientStream = duplexStreamingCall.RequestStream;
        //    var serverStream = duplexStreamingCall.ResponseStream;
        //    var responseTask = Task.Run(async () =>
        //    {
        //        await foreach (var reply in serverStream.ReadAllAsync())
        //        {
        //            Console.WriteLine($"Reply: {reply.Message}");
        //        }
        //    });
        //    foreach (var name in names)
        //    {
        //        await Task.Delay(150);
        //        Console.WriteLine($"Request: {name}");
        //        await clientStream.WriteAsync(new HelloRequest { Name = name });
        //    }
        //    await clientStream.CompleteAsync();
        //    await responseTask;
        //}

        static async Task ServerStreamWithCancellationAsync(GrpcChannel channel)
        {
            var client = new Greeter.Greeter.GreeterClient(channel);
            var request = new HelloRequest
            {
                Name = "Peter Parker"
            };
            Console.WriteLine("=== SayHelloAgain ===");
            Console.WriteLine($"Request: {request.Name}");
            try
            {
                var tokenSource = new CancellationTokenSource();
                var options = new CallOptions(cancellationToken: tokenSource.Token);
                var serverCall = client.SayHelloAgain(request, options);
                var serverStream = serverCall.ResponseStream;
                int count = 0;
                while (await serverStream.MoveNext())
                {
                    var reply = serverStream.Current;
                    Console.WriteLine($"Reply: {reply.Message}");
                    if (++count >= 3)
                    {
                        tokenSource.Cancel();
                    }
                }
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Cancelled");
            }
        }
    }
}
