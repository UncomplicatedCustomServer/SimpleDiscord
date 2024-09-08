using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("CHANNEL_PINS_UPDATE")]
    public class ChannelPinsUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    { }
}
