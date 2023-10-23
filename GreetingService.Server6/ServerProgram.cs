using ProtoBuf.Grpc.Server;

namespace GreetingService.Server6
{
    public class ServerProgram
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCodeFirstGrpc();

            var app = builder.Build();

            app.MapGrpcService<GreetingService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                                  "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}