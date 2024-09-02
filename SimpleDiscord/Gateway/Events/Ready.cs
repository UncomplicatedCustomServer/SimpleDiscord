using Newtonsoft.Json;
using SimpleDiscord.Components.Communicator;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
#pragma warning disable IDE1006
    internal class Ready(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "READY";

        public ReadyFields ready { get; internal set; }

        public override void Init() => ready = JsonConvert.DeserializeObject<ReadyFields>(RawData);
    }
}
