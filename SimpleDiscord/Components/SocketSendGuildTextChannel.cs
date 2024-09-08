using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendGuildTextChannel(string name, int? position = null, Overwrite[]? permissionOverwrites = null, string? topic = null, int? rateLimitPerUser = null, long? parentId = null, bool? nsfw = null, int? defaultAltoArchiveDuration = null, int? defaultThreadRateLimitPerUser = null) : SocketSendGuildChannel(name, ChannelType.GUILD_TEXT, position, permissionOverwrites)
    {
        public new int Type => (int)ChannelType.GUILD_TEXT;

        public string? Topic { get; } = topic;

        public int? RateLimitPerUser { get; } = rateLimitPerUser;

        public long? ParentId { get; } = parentId;

        public bool? Nsfw { get; } = nsfw;

        public int? DefaultAltoArchiveDuration { get; } = defaultAltoArchiveDuration;

        public int? DefaultThreadRateLimitPerUser { get; } = defaultThreadRateLimitPerUser;
    }
}
