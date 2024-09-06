namespace SimpleDiscord.Components
{
    public class Poll(Message message, SocketPoll socketPoll) : SocketPoll(socketPoll)
    {
        public Message Message { get; } = message;

        public void End() { }
    }
}
