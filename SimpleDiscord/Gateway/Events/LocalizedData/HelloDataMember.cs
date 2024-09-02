using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class HelloDataMember(uint heartbeatInterval)
    {
        [JsonProperty("heartbeat_interval")]
        public uint HeartbeatInterval { get; } = heartbeatInterval;
    }
}
