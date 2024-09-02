namespace SimpleDiscord.Components
{
    public class SocketPartialChannel
    {
        public long Id { get; }

        public int Type { get; }

        public SocketPartialChannel(long id, int type)
        {
            Id = id;
            Type = type;
        }
    }
}
