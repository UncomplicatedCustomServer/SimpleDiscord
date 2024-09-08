using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Activity(string name, int type, string? url = null, long? createdAt = null, string? details = null, string? state = null, Emoji? emoji = null, ActivityAsset? assets = null, ActivityButton[]? buttons = null)
    {
        public string Name { get; set; } = name;

        public int Type { get; set; } = type;

        public string? Url { get; internal set; } = url;

        [JsonProperty("created_at")]
        public long? CreatedAt { get; internal set; } = createdAt;

        public string? Details { get; internal set; } = details;

        public string? State { get; internal set; } = state;

        public Emoji? Emoji { get; internal set; } = emoji;

        public ActivityAsset? Assets { get; internal set; } = assets;

        public ActivityButton[]? Buttons { get; internal set; } = buttons;
    }
}
