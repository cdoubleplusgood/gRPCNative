using System;
using System.Threading.Tasks;
using GreetingService.Common;

namespace GreetingService.Server
{
    internal class GreetingService: IGreetingService
    {
        public Task<GreetingResponse> Greet(GreetingRequest request)
        {
            Console.WriteLine($"GreetRequest: {request.Greeting.FirstName} {request.Greeting.LastName}");
            var response = new GreetingResponse
            {
                Response = $"Hello {request.Greeting.FirstName} {request.Greeting.LastName}"
            };
            Console.WriteLine($"GreetResponse: {response.Response}");
            return Task.FromResult(response);
        }
    }
}
