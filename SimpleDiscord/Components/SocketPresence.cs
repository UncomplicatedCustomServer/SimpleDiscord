using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class SocketPresence
    {
        [JsonProperty("user")]
        public SocketUser User { get; }

        [JsonProperty("guild_id")]
        public long GuildId { get; }

        public string Status { get; }

        public UserActivity[] Activities { get; }

        [JsonProperty("client_status")]
        public string ClientStatus { get; }

        [JsonConstructor]
        public SocketPresence(SocketUser user, long guildId, string status, UserActivity[] activities, string clientStatus)
        {
            User = user;
            GuildId = guildId;
            Status = status;
            Activities = activities;
            ClientStatus = clientStatus;
        }

        public SocketPresence(SocketPresence self)
        {
            User = self.User;
            GuildId = self.GuildId;
            Status = self.Status;
            Activities = self.Activities;
            ClientStatus = self.ClientStatus;
        }
    }
}
