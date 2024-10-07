using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketVoiceState
    {
        [JsonProperty("channel_id")]
        public long? ChannelId { get; }

        [JsonProperty("user_id")]
        public long UserId { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

        public SocketMember? Member { get; }

        [JsonProperty("session_id")]
        public string SessionId { get; }

        public bool Deaf { get; }

        public bool Mute { get; }

        public bool SelfDeaf { get; }

        public bool SelfMute { get; }

        public bool SelfStream { get; }

        public bool Suppress { get; }

        [JsonProperty("request_to_speak_timestamp")]
        public string? RequestToSpeakTimestamp { get; }

        [JsonConstructor]
        public SocketVoiceState(long channelId, long userId, long? guildId, SocketMember? member, string sessionId, bool deaf, bool mute, bool selfDeaf, bool selfMute, bool selfStream, bool suppress, string? requestToSpeakTimestamp)
        {
            ChannelId = channelId;
            UserId = userId;
            GuildId = guildId;
            Member = member;
            SessionId = sessionId;
            Deaf = deaf;
            Mute = mute;
            SelfDeaf = selfDeaf;
            SelfMute = selfMute;
            SelfStream = selfStream;
            Suppress = suppress;
            RequestToSpeakTimestamp = requestToSpeakTimestamp;
        }

        public SocketVoiceState(SocketVoiceState self)
        {
            ChannelId = self.ChannelId;
            UserId = self.UserId;
            GuildId = self.GuildId;
            Member = self.Member;
            SessionId = self.SessionId;
            Deaf = self.Deaf;
            Mute = self.Mute;
            SelfDeaf = self.SelfDeaf;
            SelfMute = self.SelfMute;
            SelfStream = self.SelfStream;
            Suppress = self.Suppress;
            RequestToSpeakTimestamp = self.RequestToSpeakTimestamp;
        }
    }
}
