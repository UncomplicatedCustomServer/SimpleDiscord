using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    public class MessageAddReactionDataMember(long userId, long channelId, long messageId, long? guildId, SocketMember? member, Emoji emoji, long? messageAuthorId, bool burst, string[]? burstColors, int type)
    {
        [JsonProperty("user_id")]
        public long UserId { get; } = userId;

        [JsonProperty("channel_id")]
        public long ChannelId { get; } = channelId;

        [JsonProperty("message_id")]
        public long MessageId { get; } = messageId;

        [JsonProperty("guild_id")]
        public long? GuildId { get; } = guildId;

        public SocketMember? Member { get; } = member;

        public Emoji Emoji { get; } = emoji;

        public long? MessageAuthorId { get; } = messageAuthorId;

        public bool Burst { get; } = burst;

        public string[]? BurstColors { get; } = burstColors;

        public int Type { get; } = type;
    }
}
