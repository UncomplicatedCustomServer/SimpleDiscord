using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedBanMember(long guildId, SocketUser user) : BanMember(guildId, user)
    {
        public Guild Guild { get; } = Guild.GetGuild(guildId);
    }
}
