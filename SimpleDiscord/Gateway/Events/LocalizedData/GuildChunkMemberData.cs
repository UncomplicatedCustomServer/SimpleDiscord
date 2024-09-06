using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    internal class GuildChunkMemberData(long guildId)
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;

        public int Limit => 0;

        public string Query => string.Empty;
    }
}
