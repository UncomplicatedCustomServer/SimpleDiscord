using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketScheduledEvent
    {
        public long Id { get; }

        [JsonProperty("guild_id")]
        public long GuildId { get; }

        public string Name { get; }

        [JsonProperty("privacy_level")]
        public int PrivacyLevel { get; }

        public int Status { get; }

        [JsonProperty("entity_type")]
        public int EntityType { get; }

        [JsonProperty("entity_id")]
        public long? EntityId { get; }

        [JsonConstructor]
        public SocketScheduledEvent(long id, long guildId, string name, int privacyLevel, int status, int entityType, long? entityId)
        {
            Id = id;
            GuildId = guildId;
            Name = name;
            PrivacyLevel = privacyLevel;
            Status = status;
            EntityType = entityType;
            EntityId = entityId;
        }
    }
}
