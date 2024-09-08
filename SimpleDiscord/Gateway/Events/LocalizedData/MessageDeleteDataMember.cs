using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageDeleteDataMember(long id, long channelId, long? guildId)
    {
        public long Id { get; } = id;

        [JsonProperty("channel_id")]
        public long ChannelId { get; } = channelId;

        [JsonProperty("guild_id")]
        public long? GuildId { get; } = guildId;
    }
}
