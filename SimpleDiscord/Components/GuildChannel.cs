using Newtonsoft.Json.Linq;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Guild = Guild.GetSafeGuild(socketChannel.GuildId);

            if (socketChannel.ParentId is not null)
                Parent = Guild.GetChannel((long)socketChannel.ParentId);

            if (pushUpdate)
                Guild.SafeUpdateChannel(this);

            GuildChannel instance = List.FirstOrDefault(guildChannel => guildChannel.Id == Id && guildChannel.GuildId == GuildId);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }

        internal override void Dispose()
        {
            List.Remove(this);
            base.Dispose();
        }

        public Task<GuildChannel> Update(SocketSendGuildChannel newChannel, string? reason = null) => Edit(newChannel, reason);

        public Task<GuildChannel> Edit(SocketSendGuildChannel newChannel, string? reason = null) => Guild.Client.RestHttp.ChannelEdit(this, newChannel, reason);

        public Task Delete(string? reason = null) => Guild.Client.RestHttp.ChannelDelete(this, reason);

        public static GuildChannel? Caster(JObject obj)
        {
            SocketGuildChannel? channel = obj.ToObject<SocketGuildChannel>();

            if (channel is null)
                return null;

#nullable disable

            // Ora dal tipo deduciamo quale canale andiamo ad avere
            GuildChannel realChannel = (ChannelType)channel.Type switch
            {
                ChannelType.GUILD_TEXT or ChannelType.GUILD_ANNOUNCEMENT => new GuildTextChannel(obj.ToObject<SocketGuildTextChannel>(), true),
                ChannelType.GUILD_VOICE => new GuildVoiceChannel(obj.ToObject<SocketGuildVoiceChannel>(), true),
                ChannelType.PRIVATE_THREAD or ChannelType.PUBLIC_THREAD => new GuildThreadChannel(obj.ToObject<SocketGuildThreadChannel>(), true),
                _ => new GuildChannel(channel)
            };
            
            return realChannel;
        }

        public override string ToString() => $"<#{Id}>";

        public override bool Equals(object obj) => obj is GuildChannel channel ? Id == channel.Id : base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();
    }
}