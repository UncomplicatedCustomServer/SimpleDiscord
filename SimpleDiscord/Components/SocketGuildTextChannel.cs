using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildTextChannel : SocketGuildChannel
    {
        public string? Topic { get; }

        public bool? Nsfw { get; }

        public long? LastMessageId { get; }

        public int? RateLimitPerUser { get; }

        public int? TotalMessageSent { get; }

        [JsonConstructor]
        public SocketGuildTextChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, long? parentId, string permissions, int? flags, int? totalMessageSent) : base(id, type, guildId, position, permissionOverwrites, name, parentId, permissions, flags)
        {
            Topic = topic;
            Nsfw = nsfw;
            LastMessageId = lastMessageId;
            RateLimitPerUser = rateLimitPerUser;
            TotalMessageSent = totalMessageSent;
        }

        public SocketGuildTextChannel(GuildTextChannel channel) : base(channel)
        {
            Topic = channel.Topic;
            Nsfw = channel.Nsfw;
            LastMessageId = channel.LastMessageId;
            RateLimitPerUser = channel.RateLimitPerUser;
            TotalMessageSent = channel.TotalMessageSent;
        }

        public SocketGuildTextChannel(SocketGuildChannel baseChannel, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, int? totalMessageSent) : base(baseChannel)
        {
            Topic = topic;
            Nsfw = nsfw;
            LastMessageId = lastMessageId;
            RateLimitPerUser = rateLimitPerUser;
            TotalMessageSent = totalMessageSent;
        }

        public SocketGuildTextChannel(SocketGuildTextChannel baseChannel) : this(baseChannel, baseChannel.Topic, baseChannel.Nsfw, baseChannel.LastMessageId, baseChannel.RateLimitPerUser, baseChannel.TotalMessageSent)
        { }
    }
}
