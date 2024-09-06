using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_MEMBER_ADD")]
    public class MemberAdd(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MemberAddDataMember Data { get; private set; }

        public ResolvedMemberAddDataMember Member { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MemberAddDataMember>(RawData);
            Member = new(Data.Member, Data.GuildId, true);
        }
    }
}
