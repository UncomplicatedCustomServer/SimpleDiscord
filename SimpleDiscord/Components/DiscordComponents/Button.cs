using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class Button(int style, string? label, Emoji? emoji, string? customId, string? url, bool? disabled = false) : GenericComponent
    {
        public override int Type => (int)ComponentType.Button;

        public int Style { get; } = style;

        public string? Label { get; } = label;

        public Emoji? Emoji { get; } = emoji;

        public string? CustomId { get; } = customId;

        public string? Url { get; } = url;

        public bool? Disabled { get; } = disabled;

        public static Button New(ButtonStyle style, string label, Emoji? emoji = null, string? customId = null, string? url = null, bool? disabled = false) => new((int)style, label, emoji, customId, url, disabled);
    }
}
