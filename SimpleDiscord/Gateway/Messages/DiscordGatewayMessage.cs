using Newtonsoft.Json;

namespace SimpleDiscord.Gateway.Messages
{
#nullable enable

    public class DiscordGatewayMessage(DiscordRawGatewayMessage raw)
    {
        public int OpCode { get; } = raw.Op;

        public object? Data { get; } = raw.D;

        public int? Sequence { get; } = raw.S;

        public string? EventName { get; } = raw.T;

        [JsonIgnore]
        public string Raw { get; } = raw.Raw;

        public readonly DiscordRawGatewayMessage rawMessage = raw;
    }
}
