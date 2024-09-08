using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent(1)]
    public class Heartbeat(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    { }
}
