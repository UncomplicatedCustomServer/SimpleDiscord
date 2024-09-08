using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedMemberUpdateDataMember
    {
        public Guild Guild { get; }

        public Member Member { get; }

        public ResolvedMemberUpdateDataMember(MemberUpdateDataMember original, bool forcePush = false)
        {
            Guild = Guild.GetGuild(original.GuildId);
            Member = new(Guild, original);

            if (forcePush)
                Guild.SafeUpdateMember(Member);
        }
    }
}
