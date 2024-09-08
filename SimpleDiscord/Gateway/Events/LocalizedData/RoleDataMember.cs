using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class RoleDataMember(Role role, long guildId)
    {
        public Role Role { get; } = role;

        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;
    }
}
