using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketInteraction : ClientChild
    {
        [JsonProperty("id")]
        public long Id { get; }

        [JsonProperty("application_id")]
        public long ApplicationId { get; }

        [JsonProperty("type")]
        public int Type { get; }

        [JsonProperty("data")]
        public object? Data { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

        [JsonProperty("channel_id")]
        public long? ChannelId { get; }

        public SocketMember Member { get; }

        public string Token { get; }

        public int Version { get; }

        public SocketMessage? Message { get; }

        public string? Locale { get; }

        [JsonProperty("guild_locale")]
        public string? GuildLocale { get; }

        public int? Context { get; }

        [JsonConstructor]
        public SocketInteraction(long id, long applicationId, int type, object? data, long? guildId, long? channelId, SocketMember member, string token, int version, SocketMessage? message, string? locale, string? guildLocale, int? context)
        {
            Id = id;
            ApplicationId = applicationId;
            Type = type;
            Data = data;
            GuildId = guildId;
            ChannelId = channelId;
            Member = member;
            Token = token;
            Version = version;
            Message = message;
            Locale = locale;
            GuildLocale = guildLocale;
            Context = context;
        }

        public SocketInteraction(SocketInteraction self)
        {
            Id = self.Id;
            ApplicationId = self.ApplicationId;
            Type = self.Type;
            Data = self.Data;
            GuildId = self.GuildId;
            ChannelId = self.ChannelId;
            Member = self.Member;
            Token = self.Token;
            Version = self.Version;
            Message = self.Message;
            Locale = self.Locale;
            GuildLocale = self.GuildLocale;
            Context = self.Context;
        }
    }
}
