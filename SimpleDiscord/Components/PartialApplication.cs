namespace SimpleDiscord.Components
{
    public class PartialApplication(long id, int? flags)
    {
        public long Id { get; } = id;

        public int? Flags { get; } = flags;
    }
}
