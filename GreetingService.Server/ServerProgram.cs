using System;
using System.Collections.Generic;
using System.IO;
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
            var serverCert = File.ReadAllText("ssl/localhost-chain.crt");
            var serverKey = File.ReadAllText("ssl/localhost.key");
            var keyPair = new KeyCertificatePair(serverCert, serverKey);
            var caCert = File.ReadAllText("ssl/dev-root-ca.crt");
            var credentials = new SslServerCredentials(new List<KeyCertificatePair> { keyPair },
                caCert, SslClientCertificateRequestType.RequestAndRequireAndVerify);

            Grpc.Core.Server server = new Grpc.Core.Server
            {
                Ports = { new ServerPort("localhost", port, credentials) }
            };
            server.Services.AddCodeFirst(new GreetingService());
            server.Start();

            Console.WriteLine($"Server listening on port {port}");
            Console.ReadKey();

            await server.ShutdownAsync();
        }
    }
}
