using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class ChannelUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "CHANNEL_UPDATE";

        public SocketGenericChannel Data { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketGenericChannel>(RawData);
        }
    }
}
