using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class RoleDataMember(Role role, long guildId)
    {
        public Role Role { get; } = role;

        public long GuildId { get; } = guildId;
    }
}
