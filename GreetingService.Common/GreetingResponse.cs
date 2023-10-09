using ProtoBuf;

namespace GreetingService.Common
{
    [ProtoContract]
    public class GreetingResponse
    {
        [ProtoMember(1)]
        public string Response { get; set; }
    }
}
