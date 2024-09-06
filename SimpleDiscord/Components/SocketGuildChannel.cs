using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildChannel : SocketChannel
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; internal set;  }

        public int? Position { get; }

        [JsonProperty("permission_overwrites")]
        public Overwrite[]? PermissionOverwrites { get; }

        public string? Name { get; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; }

        public string? Permissions { get; }

        public int? Flags { get; }

        [JsonConstructor]
        public SocketGuildChannel(long id, int type, long guildId, int? position, Overwrite[]? permissionOverwrites, string? name, long? parentId, string? permissions, int? flags) : base(id, type)
        {
            GuildId = guildId;
            Position = position;
            PermissionOverwrites = permissionOverwrites;
            Name = name;
            ParentId = parentId;
            Permissions = permissions;
            Flags = flags;
        }

        public SocketGuildChannel(SocketGuildChannel channel) : base(channel.Id, channel.Type)
        {
            GuildId = channel.GuildId;
            Position = channel.Position;
            Permissions = channel.Permissions;
            Flags = channel.Flags;
            PermissionOverwrites = channel.PermissionOverwrites;
            Name = channel.Name;
            ParentId = channel.ParentId;
        }
    }
}
