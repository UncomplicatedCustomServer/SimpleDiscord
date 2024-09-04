using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_CREATE")]
    public class MessageCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketMessage Data { get; internal set; }

        public Message Message { get; internal set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketMessage>(RawData);
            Message = new(Data);
        }
    }
}