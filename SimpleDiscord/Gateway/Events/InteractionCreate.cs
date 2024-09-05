using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("INTERACTION_CREATE")]
    public class InteractionCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketInteraction Data { get; private set; }

        public Interaction Interaction { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketInteraction>(RawData);
            Interaction = new(Data);
        }
    }
}
