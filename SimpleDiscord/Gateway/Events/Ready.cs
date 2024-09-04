using Newtonsoft.Json;
using SimpleDiscord.Components.Communicator;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
#pragma warning disable IDE1006
    [InternalEvent("READY")]
    internal class Ready(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public ReadyFields ready { get; internal set; }

        public override void Init() => ready = JsonConvert.DeserializeObject<ReadyFields>(RawData);
    }
}
