using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class MessageCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "MESSAGE_CREATE";

        public MessageCreateMember Data { get; private set; }

        public override void Init() => Data = JsonConvert.DeserializeObject<MessageCreateMember>(RawData);
    }
}
