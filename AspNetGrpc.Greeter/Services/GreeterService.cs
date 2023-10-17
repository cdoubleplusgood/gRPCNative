using Grpc.Core;

namespace AspNetGrpc.Greeter.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            // Simulate slow response:
            //await Task.Delay(100);
            return await Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task SayHelloAgain(HelloRequest request,
            IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var response = new HelloReply
            {
                Message = "Hello " + request.Name
            };
            const int count = 5;
            try
            {
                foreach (int _ in Enumerable.Range(0, count))
                {
                    await Task.Delay(250);
                    await responseStream.WriteAsync(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override async Task<HelloReply>
            SayHelloToAll(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            List<string> names = new();
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                names.Add(request.Name);
            }
            var response = new HelloReply
            {
                Message = "Hello " + string.Join(", ", names)
            };
            return await Task.FromResult(response);
        }

        public override async Task SayHelloToEveryone(IAsyncStreamReader<HelloRequest> requestStream,
            IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                await Task.Delay(250);
                var response = new HelloReply
                {
                    Message = "Hello " + request.Name
                };
                await responseStream.WriteAsync(response);
            }
        }
    }
}
