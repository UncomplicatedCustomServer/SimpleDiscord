using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageRemoveReactionDataMember(long userId, long channelId, long messageId, long? guildId, Emoji emoji, bool burst, int type)
    {
        [JsonProperty("user_id")]
        public long UserId { get; } = userId;

        [JsonProperty("channel_id")]
        public long ChannelId { get; } = channelId;

        [JsonProperty("message_id")]
        public long MessageId { get; } = messageId;

        [JsonProperty("guild_id")]
        public long? GuildId { get; } = guildId;

        public Emoji Emoji { get; } = emoji;

        public bool Burst { get; } = burst;

        public int Type { get; } = type;
    }
}
