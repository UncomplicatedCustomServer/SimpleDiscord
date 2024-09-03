using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuildThreadChannel))]
    public class GuildThreadChannel : GuildTextChannel, IGuildElement
    {
        public SocketMember? Owner { get; }

        public int? MessageCount { get; }

        public int? MemberCount { get; }

        public ThreadMetadata? ThreadMetadata { get; }

        public ThreadMember? Member { get; }

        public int? DefaultAutoArchiveDuration { get; }

        [JsonConstructor]
        public GuildThreadChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, long? parentId, string permissions, int? flags, string topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, int? totalMessageSent, long? owner, int? messageCount, int? memberCount, ThreadMetadata? threadMetadata, ThreadMember? member, int? defaultAutoArchiveDuration, bool safeUpdate = true) : base(id, type, guildId, position, permissionOverwrites, name, parentId, permissions, flags, topic, nsfw, lastMessageId, rateLimitPerUser, totalMessageSent, false)
        {
            if (owner is not null)
                Owner = Guild.GetMember((long)owner);

            MessageCount = messageCount;
            MemberCount = memberCount;
            ThreadMetadata = threadMetadata;
            Member = member;
            DefaultAutoArchiveDuration = defaultAutoArchiveDuration;

            if (safeUpdate)
                Guild.SafeUpdateChannel(this);
        }

        public GuildThreadChannel(GuildTextChannel baseChannel, SocketMember? owner, int? messageCount, int? memberCount, ThreadMetadata? threadMetadata, ThreadMember? member, int? defaultAutoArchiveDuration) : base(baseChannel)
        {
            Owner = owner;
            MessageCount = messageCount;
            MemberCount = memberCount;
            ThreadMetadata = threadMetadata;
            Member = member;
            DefaultAutoArchiveDuration = defaultAutoArchiveDuration;
        }

        public GuildThreadChannel(GuildThreadChannel baseChannel) : this(baseChannel, baseChannel.Owner, baseChannel.MessageCount, baseChannel.MemberCount, baseChannel.ThreadMetadata, baseChannel.Member, baseChannel.DefaultAutoArchiveDuration)
        { }

        public GuildThreadChannel(SocketGuildThreadChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            if (socketChannel.OwnerId is not null)
                Owner = Guild.GetMember((long)socketChannel.OwnerId);

            MessageCount = socketChannel.MessageCount;
            MemberCount = socketChannel.MemberCount;
            ThreadMetadata = socketChannel.ThreadMetadata;
            Member = socketChannel.Member;
            DefaultAutoArchiveDuration = socketChannel.DefaultAutoArchiveDuration;

            if (pushUpdate)
                Guild.SafeUpdateChannel(this);
        }
    }
}
