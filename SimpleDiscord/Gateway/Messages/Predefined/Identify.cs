using Newtonsoft.Json;
using SimpleDiscord.Components;
using System.Collections.Generic;

namespace SimpleDiscord.Gateway.Messages.Predefined
{
#nullable enable

    internal class Identify(string token, Dictionary<string, string> properties, int intents, bool? compress = false, int? largeThreshold = 50, int[]? shard = null, SocketPresence? presence = null)
    {
        public string Token { get; } = token;

        public Dictionary<string, string> Properties { get; } = properties;

        public bool? Compress { get; } = compress;

        public int? LargeThreshold { get; } = largeThreshold;

        [JsonIgnore]
        public int[]? Shard { get; } = shard;

        public SocketPresence? Presence { get; } = presence;

        public int Intents { get; } = intents;
    }
}
