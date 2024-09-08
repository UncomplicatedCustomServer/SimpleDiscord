using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("THREAD_MEMBERS_UPDATE")]
    public class ThreadMembersUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public ThreadMemberUpdateMember Member { get; private set; }

        public override void Init()
        {
            Member = JsonConvert.DeserializeObject<ThreadMemberUpdateMember>(RawData);

            if (Member is not null)
            {
                foreach (ThreadMember member in Member.AddedMembers)
                    Guild.GetSafeGuild(Member.GuildId).GetSafeThreadChannel(Member.Id).SafeUpdateMember(member);

                if (Member.RemovedMemberIds is not null)
                    foreach (long userId in Member.RemovedMemberIds)
                        Guild.GetSafeGuild(Member.GuildId).GetSafeThreadChannel(Member.Id).SafeDisposeMember(userId);
            }
        }
    }
}
