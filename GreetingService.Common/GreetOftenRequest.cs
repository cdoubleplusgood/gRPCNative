using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Common
{
    [ProtoContract]
    public class GreetOftenRequest
    {
        [ProtoMember(1)]
        public Greeting Greeting { get; set; } = new Greeting();

        [ProtoMember(2)]
        public int Count { get; set; }
    }
}
