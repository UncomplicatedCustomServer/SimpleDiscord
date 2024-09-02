namespace SimpleDiscord.Components
{
    public class UnavailableGuild(long id, bool unavailable)
    {
        public long Id { get; } = id;

        public bool Unavailable { get; } = unavailable;
    }
}
