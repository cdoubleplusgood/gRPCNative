using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreetingService.Common;

namespace GreetingService.Server
{
    internal class GreetingService: IGreetingService
    {
        public Task<GreetingResponse> Greet(GreetingRequest request)
        {
            Console.WriteLine("Greet");
            Console.WriteLine($"GreetRequest: {request.Greeting.FirstName} {request.Greeting.LastName}");
            var response = new GreetingResponse
            {
                Response = $"Hello {request.Greeting.FirstName} {request.Greeting.LastName}"
            };
            Console.WriteLine($"GreetResponse: {response.Response}");
            return Task.FromResult(response);
        }

        public async IAsyncEnumerable<GreetingResponse> GreetOften(GreetOftenRequest request)
        {
            Console.WriteLine("GreetOften");
            Console.WriteLine("GreetOftenRequest:" +
                              $"{request.Greeting.FirstName} {request.Greeting.LastName} " +
                              $"{request.Count}");
            var response = new GreetingResponse
            {
                Response = $"Hello {request.Greeting.FirstName} {request.Greeting.LastName}"
            };
            foreach (int _ in Enumerable.Range(0, request.Count))
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                Console.WriteLine($"GreetResponse: {response.Response}");
                yield return response;
            }
        }
    }
}
