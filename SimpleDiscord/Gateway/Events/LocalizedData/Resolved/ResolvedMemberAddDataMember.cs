using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedMemberAddDataMember : MemberAddDataMember
    {
        public Guild Guild { get; }

        public new Member Member { get; }

        public ResolvedMemberAddDataMember(SocketMember member, long guildId, bool forcePush = false) : base(member, guildId)
        {
            Guild = Guild.GetSafeGuild(guildId);
            Member = new(Guild, member);

            if (forcePush && Member is not null)
                Guild.SafeUpdateMember(Member);
        }
    }
}
