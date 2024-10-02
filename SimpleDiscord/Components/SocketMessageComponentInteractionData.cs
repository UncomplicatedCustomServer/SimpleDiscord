using Newtonsoft.Json;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketMessageComponentInteractionData : SocketInteractionData
    {
        [JsonProperty("custom_id")]
        public string CustomId { get; }

        [JsonProperty("component_type")]
        public int ComponentType { get; }

        public string[]? Values { get; }

        public SocketResolvedData? Resolved { get; }

        [JsonConstructor]
        public SocketMessageComponentInteractionData(string customId, int componentType, string[]? values, SocketResolvedData? resolved)
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
