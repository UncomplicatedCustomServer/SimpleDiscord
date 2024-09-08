namespace SimpleDiscord.Components
{
#nullable enable
    public class Presence : SocketPresence
    {
        public Guild Guild { get; }

        public Member? Member { get; }

        public Presence(SocketPresence instance, Guild? guild = null) : base(instance)
        {
            Guild = guild ?? Guild.GetSafeGuild(instance.GuildId);
            Member = Guild.GetMember(instance.User.Id);
        }
    }
}
