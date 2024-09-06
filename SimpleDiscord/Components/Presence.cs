namespace SimpleDiscord.Components
{
#nullable enable
    public class Presence : SocketPresence
    {
        public Guild Guild { get; }

        public Member? Member { get; }

        public Presence(SocketPresence instance) : base(instance)
        {
            Guild = Guild.GetSafeGuild(instance.GuildId);
            Member = Guild.GetMember(instance.User.Id);
        }
    }
}
