using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent(10)]
    internal class Hello(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public HelloDataMember Data { get; private set; }

        public override void Init() => Data = JsonConvert.DeserializeObject<HelloDataMember>(RawData);
    }
}
