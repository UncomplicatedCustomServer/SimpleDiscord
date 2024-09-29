using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("VOICE_STATE_UPDATE")]
    public class VoiceStateUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketVoiceState Data { get; internal set; }

        public override void Init() => Data = JsonConvert.DeserializeObject<SocketVoiceState>(RawData);
    }
}
