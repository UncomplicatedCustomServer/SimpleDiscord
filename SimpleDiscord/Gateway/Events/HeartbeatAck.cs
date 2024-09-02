using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class HeartbeatAck(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 11;
    }
}
