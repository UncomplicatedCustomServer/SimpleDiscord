namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageBulkDeleteDataMember(long[] id, long channelId, long? guildId)
    {
        public long[] Ids { get; } = id;

        public long ChannelId { get; } = channelId;

        public long? GuildId { get; } = guildId;
    }
}
