using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketScheduledEvent
    {
        public long Id { get; }

        public long GuildId { get; }

        public string Name { get; } 

        public int PrivacyLevel { get; }

        public int Status { get; }

        public int EntityType { get; }

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
