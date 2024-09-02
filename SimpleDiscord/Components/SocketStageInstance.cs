namespace SimpleDiscord.Components
{
    public class SocketStageInstance
    { 
        public long Id { get; }

        public long GuildId { get; }

        public long ChannelId { get; }

        public string Topic { get; }

        public int PrivacyLevel { get; }

        public bool DiscoverableDisabled { get; }

        public long? GuildScheduledEventId { get; }

        public SocketStageInstance(long id, long guildId, long channelId, string topic, int privacyLevel, bool discoverableDisabled, long? guildScheduledEventId)
        {
            Id = id;
            GuildId = guildId;
            ChannelId = channelId;
            Topic = topic;
            PrivacyLevel = privacyLevel;
            DiscoverableDisabled = discoverableDisabled;
            GuildScheduledEventId = guildScheduledEventId;
        }
    }
}
