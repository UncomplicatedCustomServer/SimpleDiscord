using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Poll(Message message, SocketPoll socketPoll) : SocketPoll(socketPoll)
    {
        public Message Message { get; } = message;

        public async Task<SocketMessage?> End()
        {
            if (Message.Author.Id != Message.Client.Bot.Id)
                return null;

            return await Message.Client.RestHttp.EndPoll(this);
        }
    }
}
