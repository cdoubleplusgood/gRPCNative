using ProtoBuf;

namespace GreetingService.Common
{
    [ProtoContract]
    public class GreetingRequest
    {
        [ProtoMember(1)]
        public Greeting Greeting { get; set; } = new Greeting();
    }
}
