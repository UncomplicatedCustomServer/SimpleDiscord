using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildTextChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, string topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, long? parentId, int? messageCount, int memberCount, string permissions, int? flags, int? totalMessageSent) : SocketGenericChannel(id, type)
    {
        public long GuildId { get; } = guildId;

        public int? Position { get; } = position;

        public Overwrite[] PermissionOverwrites { get; } = permissionOverwrites;

        public string? Name { get; } = name;

        public string? Topic { get; } = topic;

        public bool? Nsfw { get; } = nsfw;

        public long? LastMessageId { get; } = lastMessageId;

        public int? RateLimitPerUser { get; } = rateLimitPerUser;

        public long? ParentId { get; } = parentId;

        public int? MessageCount { get; } = messageCount;

        public int MemberCount { get; } = memberCount;

        public string? Permissions { get; } = permissions;

        public int? Flags { get; } = flags;

        public int? TotalMessageSent { get; } = totalMessageSent;

        public async Task<SocketMessage> SendMessage(string content) => await Client.Instance.RestHttp.SendMessage(this, new(content));

        public async Task<SocketMessage> SendMessage(SocketSendMessage msg) => await Client.Instance.RestHttp.SendMessage(this, msg);
    }
}
