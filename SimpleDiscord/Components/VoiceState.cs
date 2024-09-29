using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class VoiceState : SocketVoiceState
    {
        public new Member? Member { get; }

        public Guild Guild { get; }

        public GuildVoiceChannel? Channel { get; }

        public SocketUser? User => Member?.User;

        public bool Connected => ChannelId is not 0 && ChannelId is not null;

#nullable disable
        public VoiceState(SocketVoiceState socket) : base(socket)
        {
            Guild = Guild.GetSafeGuild((long)socket.GuildId);
            Channel = Guild.GetSafeChannel((long)socket.ChannelId) as GuildVoiceChannel;

            if (socket.Member is not null && Guild is not null)
                Member = new(Guild, socket.Member, this);
        }
    }
}
