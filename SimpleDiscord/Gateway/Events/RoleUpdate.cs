using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_ROLE_UPDATE")]
    public class RoleUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public ResolvedRoleDataMember Data { get; private set; }

        public override void Init() => Data = new(JsonConvert.DeserializeObject<RoleDataMember>(RawData), true);
    }
}
