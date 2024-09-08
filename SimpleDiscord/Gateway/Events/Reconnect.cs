using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent(7)]
    public class Reconnect(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    { }
}
