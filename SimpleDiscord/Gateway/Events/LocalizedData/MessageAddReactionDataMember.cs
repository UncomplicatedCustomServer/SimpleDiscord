using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    public class MessageAddReactionDataMember(long userId, long channelId, long messageId, long? guildId, SocketMember? member, Emoji emoji, long? messageAuthorId, bool burst, string[]? burstColors, int type)
    {
        public long UserId { get; } = userId;

        public long ChannelId { get; } = channelId;

        public long MessageId { get; } = messageId;

        public long? GuildId { get; } = guildId;

        public SocketMember? Member { get; } = member;

        public Emoji Emoji { get; } = emoji;

        public long? MessageAuthorId { get; } = messageAuthorId;

        public bool Burst { get; } = burst;

        public string[]? BurstColors { get; } = burstColors;

        public int Type { get; } = type;
    }
}
