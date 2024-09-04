using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    internal class INTERACTION_CREATE(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "INTERACTION_CREATE";
    }
}
