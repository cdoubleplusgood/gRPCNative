using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreetingService.Common
{
    [ProtoContract]
    public class GreetingResponse
    {
        [ProtoMember(1)]
        public string Response { get; set; }
    }
}
