using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketStageInstance
    { 
        public long Id { get; }

        [JsonProperty("guild_id")]
        public long GuildId { get; }

        [JsonProperty("channel_id")]
        public long ChannelId { get; }

        public string Topic { get; }

        [JsonProperty("privacy_level")]
        public int PrivacyLevel { get; }

        [JsonProperty("discoverable_disabled")]
        public bool DiscoverableDisabled { get; }

        [JsonProperty("guild_scheduled_event_id")]
        public long? GuildScheduledEventId { get; }

        [JsonConstructor]
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
