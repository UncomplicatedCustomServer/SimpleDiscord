using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent(9)]
    internal class InvalidSession(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public bool IsResumeable { get; internal set; } = false;

        public override void Init()
        {
            if (bool.TryParse(RawData, out bool resumeable))
                IsResumeable = resumeable;
        }
    }
}
