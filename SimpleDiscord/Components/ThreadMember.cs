namespace SimpleDiscord.Components
{
#nullable enable
    public class ThreadMember(long? id, long? userId, string? joinTimestamp, SocketMember? member)
    {
        public long? Id { get; } = id;

        public long? UserId { get; } = userId;

        public string? JoinTimestamp { get; } = joinTimestamp;

        public SocketMember? Member { get; internal set; } = member;
    }
}
