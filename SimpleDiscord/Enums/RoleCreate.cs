using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Enums
{
    [InternalEvent("GUILD_ROLE_CREATE")]
    public class RoleCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public ResolvedRoleDataMember Data { get; private set; }

        public override void Init() => Data = new(JsonConvert.DeserializeObject<RoleDataMember>(RawData), true);
    }
}
