using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent(11)]
    internal class HeartbeatAck(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    { }
}
