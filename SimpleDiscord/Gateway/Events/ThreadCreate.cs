using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("THREAD_CREATE")]
    public class ThreadCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketGuildThreadChannel Data { get; private set; }

        public GuildThreadChannel Thread { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketGuildThreadChannel>(RawData);
            Thread = new(Data, true);
        }
    }
}
