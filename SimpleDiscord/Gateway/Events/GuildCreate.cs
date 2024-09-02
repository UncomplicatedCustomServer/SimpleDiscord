using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class GuildCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "GUILD_CREATE";

        public GuildCreatedMember Data { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<GuildCreatedMember>(RawData);
        }
    }
}
