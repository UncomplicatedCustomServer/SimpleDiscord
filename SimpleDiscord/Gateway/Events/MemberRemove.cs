using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_MEMBER_REMOVE")]
    public class MemberRemove(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MemberRemoveDataMember Data { get; private set; }

        public ResolvedMemberRemoveDataMember Member { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MemberRemoveDataMember>(RawData);
            Member = new(Data.GuildId, Data.User, true);
        }
    }
}
