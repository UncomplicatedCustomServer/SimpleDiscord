using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_MEMBER_UPDATE")]
    public class MemberUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MemberUpdateDataMember Data { get; private set; }

        public ResolvedMemberUpdateDataMember Member { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MemberUpdateDataMember>(RawData);
            Member = new(Data, true);
        }
    }
}
