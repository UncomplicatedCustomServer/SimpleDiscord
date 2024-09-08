using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("PRESENCE_UPDATE")]
    public class PresenceUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketPresence Data { get; private set; }

        public Presence Presence { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketPresence>(RawData);
            Presence = new(Data);
        }
    }
}
