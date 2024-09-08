using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_ROLE_CREATE")]
    public class RoleCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public RoleDataMember Data { get; private set; }

        public ResolvedRoleDataMember Role { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<ResolvedRoleDataMember>(RawData);
            Role = new(Data, true);
        }
    }
}
