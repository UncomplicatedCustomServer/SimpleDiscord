using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketMessageObject
    {
        [JsonProperty("channel_id")]
        public long ChannelId { get; }

        [JsonProperty("message_id")]
        public long MessageId { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

        [JsonConstructor]
        public SocketMessageObject(long channelId, long messageId, long? guildId)
        {
            ChannelId = channelId;
            MessageId = messageId;
            GuildId = guildId;
        }

        public SocketMessageObject(SocketMessageObject self)
        {
            ChannelId = self.ChannelId;
            MessageId = self.MessageId;
            GuildId = self.GuildId;
        }
    }
}
