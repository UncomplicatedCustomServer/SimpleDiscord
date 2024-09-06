namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageDeleteDataMember(long id, long channelId, long? guildId)
    {
        public long Id { get; } = id;

        public long ChannelId { get; } = channelId;

        public long? GuildId { get; } = guildId;
    }
}
