using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_UPDATE")]
    public class MessageUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketMessage Data { get; internal set; }

        public Message Message { get; internal set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketMessage>(RawData);
            Message = new(Data);

            // We might update the message on the channel
            Message.Channel.SafeUpdateMessage(Message); // The check is implicit
        }
    }
}
