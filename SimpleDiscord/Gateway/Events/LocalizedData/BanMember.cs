using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class BanMember(long guildId, SocketUser user)
    {
        public long GuildId { get; } = guildId;

        public SocketUser User { get; } = user;
    }
}
