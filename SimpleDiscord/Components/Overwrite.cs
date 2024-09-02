namespace SimpleDiscord.Components
{
    public class Overwrite(long id, int type, string allow, string deny)
    {
        public long Id { get; } = id;

        public int Type { get; } = type;

        public string Allow { get; } = allow;

        public string Deny { get; } = deny;
    }
}
