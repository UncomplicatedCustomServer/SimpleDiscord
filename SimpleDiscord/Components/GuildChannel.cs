using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuildChannel))]
    public class GuildChannel : SocketGuildChannel, IGuildElement
    {
        public static readonly List<GuildChannel> List = [];

        public new ChannelType Type { get; }

        public Guild Guild { get; }

        public GuildChannel? Parent { get; }

        [JsonConstructor]
        public GuildChannel(long id, int type, long guildId, int? position, Overwrite[]? permissionOverwrites, string? name, long? parentId, string? permissions, int? flags, bool safeUpdate = true) : base(id, type, guildId, position, permissionOverwrites, name, parentId, permissions, flags)
        {
            GuildChannel instance = List.FirstOrDefault(guildChannel => guildChannel.Id == id && guildChannel.GuildId == guildId);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);

            Guild = Client.Instance.GetSafeGuild(guildId);
            if (parentId is not null)
                Parent = Guild.GetChannel((long)parentId);

            Type = (ChannelType)type;

            if (safeUpdate)
                Guild.SafeUpdateChannel(this);
        }

        public GuildChannel(GuildChannel self) : base(self.Id, (int)self.Type, self.GuildId, self.Position, self.PermissionOverwrites, self.Name, self.ParentId, self.Permissions, self.Flags)
        {
            Type = self.Type;
            Guild = self.Guild;
            Parent = self.Parent;
        }

        public GuildChannel(SocketGuildChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            GuildChannel cached = List.FirstOrDefault(c => c.Id == socketChannel.Id && c.GuildId == socketChannel.GuildId);
            if (cached is not null)
            {
                Type = cached.Type;
                Guild = cached.Guild;
                Parent = cached.Parent;
                return;
            }

            Type = (ChannelType)socketChannel.Type;
            Guild = Client.Instance.GetSafeGuild(socketChannel.GuildId);
            if (socketChannel.ParentId is not null)
                Parent = Guild.GetChannel((long)socketChannel.ParentId);

            Console.WriteLine($"\n\nWE HAVE {Client.Instance.Guilds.Count} REGISTERED GUILDS - this id is {socketChannel.GuildId}\n\n");
            if (pushUpdate)
                Guild.SafeUpdateChannel(this);

            GuildChannel instance = List.FirstOrDefault(guildChannel => guildChannel.Id == Id && guildChannel.GuildId == GuildId);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }
    }
}