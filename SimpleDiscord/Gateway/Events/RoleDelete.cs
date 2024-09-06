using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_ROLE_DELETE")]
    public class RoleDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public RoleDeleteDataMember Data { get; private set; }

        public ResolvedRoleDeleteDataMember Role { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<RoleDeleteDataMember>(RawData);
            Role = new(Data.GuildId, Data.RoleId, true);
        }
    }
}
