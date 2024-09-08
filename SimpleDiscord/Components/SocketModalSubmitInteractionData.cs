using Newtonsoft.Json;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketModalSubmitInteractionData : SocketInteractionData
    {
        [JsonProperty("custom_id")]
        public string CustomId { get; }

        public TextInput[]? Components { get; }

        [JsonConstructor]
        public SocketModalSubmitInteractionData(string customId, TextInput[]? components)
        {
            CustomId = customId;
            Components = components;
        }

        public SocketModalSubmitInteractionData(SocketModalSubmitInteractionData self)
        {
            CustomId = self.CustomId;
            Components = self.Components;
        }
    }
}
