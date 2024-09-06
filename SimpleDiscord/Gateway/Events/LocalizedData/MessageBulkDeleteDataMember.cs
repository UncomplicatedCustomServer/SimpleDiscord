using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageBulkDeleteDataMember(long[] id, long channelId, long? guildId)
    {
        public long[] Ids { get; } = id;

        [JsonProperty("channel_id")]
        public long ChannelId { get; } = channelId;

        [JsonProperty("guild_id")]
        public long? GuildId { get; } = guildId;
    }
}
