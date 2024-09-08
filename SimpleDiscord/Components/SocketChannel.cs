using SimpleDiscord.Components.Attributes;

namespace SimpleDiscord.Components
{
    [EndpointInfo("/channels/{channel.id}", "CHANNEL")]
    public class SocketChannel(long id, int type) : DisposableElement
    {
        public long Id { get; } = id;

        public int Type { get; } = type;
    }
}
