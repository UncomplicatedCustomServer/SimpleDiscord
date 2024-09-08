using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events
{
    public class PollEnded(Message message, SocketPoll poll)
    {
        public Poll Poll { get; } = new(message, poll);
    }
}
