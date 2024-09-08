using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDiscord.Extensions;
using System;
using System.Collections.Generic;

namespace SimpleDiscord.Gateway.Messages
{
#nullable enable

    public class DiscordRawGatewayMessage
    {
        public int Op { get; }

        public object? D { get; }

        public int? S { get; }

        public string? T { get; }

        [JsonIgnore]
        public string Raw { get; }

        public DiscordRawGatewayMessage(string raw, int op, object? d = null, int? s = null, string? t = null)
        {
            Op = op;
            D = d;
            S = s;
            T = t;
            Raw = raw;
        }

        public DiscordRawGatewayMessage(string raw, Dictionary<string, object> el)
        {
            Op = int.Parse(el.ForceGet("op")?.ToString());
            D = el.ForceGet("d")?.ToString();

            if (el.ForceGet("s") is not null)
                S = int.Parse(el.ForceGet("s")?.ToString());
            else
                S = null;

            T = el.ForceGet("t")?.ToString();
            Raw = raw;
        }
    }
}
