using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildThreadChannel : SocketGuildTextChannel
    {
        public long? OwnerId { get; }

        public int? MessageCount { get; }

        public int? MemberCount { get; }

        public ThreadMetadata? ThreadMetadata { get; }

        public ThreadMember? Member { get; }

        public int? DefaultAutoArchiveDuration { get; }

        [JsonConstructor]
        public SocketGuildThreadChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, long? parentId, string permissions, int? flags, int? totalMessageSent, long? ownerId, int? messageCount, int? memberCount, ThreadMetadata? threadMetadata, ThreadMember? member, int? defaultAutoArchiveDuration) : base(id, type, guildId, position, permissionOverwrites, name, topic, nsfw, lastMessageId, rateLimitPerUser, parentId, permissions, flags, totalMessageSent)
        {
            OwnerId = ownerId;
            MessageCount = messageCount;
            MemberCount = memberCount;
            ThreadMetadata = threadMetadata;
            Member = member;
            DefaultAutoArchiveDuration = defaultAutoArchiveDuration;
        }

        public SocketGuildThreadChannel(SocketGuildTextChannel baseChannel, long? ownerId, int? messageCount, int? memberCount, ThreadMetadata? threadMetadata, ThreadMember? member, int? defaultAutoArchiveDuration) : base(baseChannel)
        {
            OwnerId = ownerId;
            MessageCount = messageCount;
            MemberCount = memberCount;
            ThreadMetadata = threadMetadata;
            Member = member;
            DefaultAutoArchiveDuration = defaultAutoArchiveDuration;
        }

        public SocketGuildThreadChannel(SocketGuildThreadChannel baseChannel) : this(baseChannel, baseChannel.OwnerId, baseChannel.MessageCount, baseChannel.MemberCount, baseChannel.ThreadMetadata, baseChannel.Member, baseChannel.DefaultAutoArchiveDuration)
        { }

        public SocketGuildThreadChannel(GuildThreadChannel baseChannel) : base(baseChannel)
        {
            OwnerId = baseChannel.Owner?.User?.Id;
            MessageCount = baseChannel.MessageCount;
            MemberCount = baseChannel.MemberCount;
            ThreadMetadata = baseChannel.ThreadMetadata;
            Member = baseChannel.Member;
            DefaultAutoArchiveDuration = baseChannel.DefaultAutoArchiveDuration;
        }
    }
}
