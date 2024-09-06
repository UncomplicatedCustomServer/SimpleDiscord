using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageRemoveReactionDataMember(long userId, long channelId, long messageId, long? guildId, Emoji emoji, bool burst, int type)
    {
        public long UserId { get; } = userId;

        public long ChannelId { get; } = channelId;

        public long MessageId { get; } = messageId;

        public long? GuildId { get; } = guildId;

        public Emoji Emoji { get; } = emoji;

        public bool Burst { get; } = burst;

        public int Type { get; } = type;
    }
}
