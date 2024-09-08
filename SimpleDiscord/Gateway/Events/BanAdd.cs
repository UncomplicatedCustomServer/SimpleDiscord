using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_BAN_ADD")]
    public class BanAdd(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public BanMember Data { get; private set; }

        public ResolvedBanMember Ban { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<BanMember>(RawData);
            Ban = new(Data.GuildId, Data.User);
            Ban.Guild.SafeClearMember(Data.User.Id);
        }
    }
}
