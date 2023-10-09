using ProtoBuf;

namespace GreetingService.Common
{
    [ProtoContract]
    public class Greeting
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }

        [ProtoMember(2)]
        public string LastName { get; set; }
    }

}
