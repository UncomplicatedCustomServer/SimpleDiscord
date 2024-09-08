using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedMemberRemoveDataMember : MemberRemoveDataMember
    {
        public Guild Guild { get; }

        public ResolvedMemberRemoveDataMember(long guildId, SocketUser user, bool forceUpdate = false) : base(guildId, user)
        {
            Guild = Guild.GetGuild(guildId);

            if (forceUpdate)
                Guild.SafeClearMember(user.Id);
        }
    }
}
