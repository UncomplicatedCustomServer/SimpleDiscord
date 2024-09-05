using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketMessageObject
    {
        public long ChannelId { get; }

        public long MessageId { get; }

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
