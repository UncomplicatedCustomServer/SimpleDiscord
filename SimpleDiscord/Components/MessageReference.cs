namespace SimpleDiscord.Components
{
    public class MessageReference(int type, long? messageId = null, long? channelId = null, long? guildId = null)
    {
        public int Type { get; } = type;

        public long? MessageId { get; } = messageId;

        public long? ChannelId { get; } = channelId;

        public long? GuildId { get; } = guildId;
    }
}
