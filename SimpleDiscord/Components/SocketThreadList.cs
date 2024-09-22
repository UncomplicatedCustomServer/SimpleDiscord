namespace SimpleDiscord.Components
{
    internal class SocketThreadList(SocketGuildThreadChannel[] threads, ThreadMember[] members)
    {
        public SocketGuildThreadChannel[] Threads { get; } = threads;

        public ThreadMember[] Members { get; } = members;
    }
}
