using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Common;

namespace GreetingService.Server
{
    internal class GreetingService: IGreetingService
    {
        public Task<GreetingResponse> Greet(GreetingRequest request)
        {
            var response = new GreetingResponse
            {
                Response = $"Hello {request.Greeting.FirstName} {request.Greeting.LastName}"
            };
            return Task.FromResult(response);
        }
    }
}
