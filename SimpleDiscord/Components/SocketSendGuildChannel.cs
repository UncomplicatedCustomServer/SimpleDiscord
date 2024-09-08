using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendGuildChannel(string name, int type, int? position = null, Overwrite[]? permissionOverwrites = null)
    {
        public string Name { get; } = name;

        public int Type { get; } = type;

        public int? Position { get; } = position;

        public Overwrite[]? PermissionOverwrites { get; } = permissionOverwrites;

        public SocketSendGuildChannel(string name, ChannelType type, int? position = null, Overwrite[]? permissionOverwrites = null) : this(name, (int)type, position, permissionOverwrites) 
        { }
    }
}
