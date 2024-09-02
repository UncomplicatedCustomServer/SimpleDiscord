using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class InvalidSession(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 9;

        public bool IsResumeable { get; internal set; } = false;

        public override void Init()
        {
            /*if (bool.TryParse(GatewayMessage.Data, out bool resumeable))
                IsResumeable = resumeable;*/
        }
    }
}
