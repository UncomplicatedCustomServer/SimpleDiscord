using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("CHANNEL_DELETE")]
    public class ChannelDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public GuildChannel Channel { get; private set; }

        public override void Init()
        {
            SocketGuildChannel channel = JsonConvert.DeserializeObject<SocketGuildChannel>(RawData);

            if (channel is null)
                return;

            if (channel.GuildId is 0)
                return;

            Channel = new(channel);
            Channel?.Guild.SafeClearChannel(Channel);
        }
    }
}
