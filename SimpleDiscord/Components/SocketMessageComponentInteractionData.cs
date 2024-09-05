﻿using Newtonsoft.Json;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketMessageComponentInteractionData : SocketInteractionData
    {
        [JsonProperty("custom_id")]
        public string CustomId { get; }

        public int ComponentType { get; }

        public SelectOption[]? Values { get; }

        public SocketResolvedData? Resolved { get; }

        [JsonConstructor]
        public SocketMessageComponentInteractionData(string customId, int componentType, SelectOption[]? values, SocketResolvedData? resolved)
        {
            CustomId = customId;
            ComponentType = componentType;
            Values = values;
            Resolved = resolved;
        }

        public SocketMessageComponentInteractionData(SocketMessageComponentInteractionData self)
        {
            CustomId = self.CustomId;
            ComponentType = self.ComponentType;
            Values = self.Values;
            Resolved = self.Resolved;
        }
    }
}
