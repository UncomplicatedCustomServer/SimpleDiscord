using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class EmojisUpdateDataMember(long guildId, Emoji[] emojis)
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;

        public Emoji[] Emojis { get; set; } = emojis;
    }
}
