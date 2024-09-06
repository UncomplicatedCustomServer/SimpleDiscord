using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedRoleDeleteDataMember : RoleDeleteDataMember
    {
        public Guild Guild { get; }

        public ResolvedRoleDeleteDataMember(long guildId, long roleId, bool forcePush = false) : base(guildId, roleId)
        {
            Guild = Guild.GetGuild(guildId);

            if (forcePush)
                Guild.SafeClearRole(roleId);
        }
    }
}
