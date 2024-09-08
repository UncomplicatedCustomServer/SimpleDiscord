using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
   public class ResolvedRoleDataMember : RoleDataMember
    {
        public Guild Guild { get; }

        public ResolvedRoleDataMember(RoleDataMember data, bool fireUpdate = false) : base(data.Role, data.GuildId)
        {
            Guild = Guild.GetGuild(data.GuildId);

            // Fire the updoot if allowed
            if (fireUpdate)
                Guild.SafeUpdateRole(Role);
        }
    }
}
