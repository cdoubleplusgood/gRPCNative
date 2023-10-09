using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ProtoBuf.Grpc.Server;

namespace GreetingService.Server
{
    internal class ServerProgram
    {
        static async Task Main()
        {
            const int port = 50051;
            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Services.AddCodeFirst(new GreetingService());
            server.Start();

            Console.WriteLine($"Server listening on port {port}");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
