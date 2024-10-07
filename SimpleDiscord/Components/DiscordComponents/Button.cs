using SimpleDiscord.Enums;
using System;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class Button(int style, string? label, Emoji? emoji, string? customId, string? url, bool? disabled = false) : GenericComponent
    {
        public override int Type => (int)ComponentType.Button;

        public int Style { get; internal set; } = style;

        public string? Label { get; internal set; } = label;

        public Emoji? Emoji { get; internal set; } = emoji;

        public string? CustomId { get; internal set; } = customId;

        public string? Url { get; internal set; } = url;

        public bool? Disabled { get; internal set; } = disabled;

        internal Action<Interaction, object>? Callback { get; set; } = null;

        internal object? Data { get; set; } = null;

        [Obsolete("You can't use anymore the Button::New function as it's not suitable anymore! Consider using the Components.Builders.ButtonBuilder class instead!", true)]
        public static Button New(ButtonStyle style, string label, Emoji? emoji = null, string? customId = null, string? url = null, bool? disabled = false) => new((int)style, label, emoji, customId, url, disabled);
    }
}
