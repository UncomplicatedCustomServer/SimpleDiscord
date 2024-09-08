using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class RoleDeleteDataMember(long guildId, long roleId)
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;

        [JsonProperty("role_id")]
        public long RoleId { get; } = roleId;
    }
}
