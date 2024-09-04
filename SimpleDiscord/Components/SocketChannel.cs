using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Networking;

namespace SimpleDiscord.Components
{
    [EndpointInfo("/channels/{channel.id}", "CHANNEL")]
    public class SocketChannel(long id, int type) : SyncableElement
    {
        public long Id { get; } = id;

        public int Type { get; } = type;
    }
}
