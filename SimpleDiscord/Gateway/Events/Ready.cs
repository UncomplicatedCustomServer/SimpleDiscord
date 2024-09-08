using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
#pragma warning disable IDE1006
    [InternalEvent("READY")]
    internal class Ready(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public ReadyDataMember Data { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<ReadyDataMember>(RawData);
        }
    }
}
