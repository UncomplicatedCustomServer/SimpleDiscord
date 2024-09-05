namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class SelectOption(string label, string value, string? description, Emoji? emoji, bool? @default)
    {
        public string Label { get; } = label;

        public string Value { get; } = value;

        public string? Description { get; } = description;

        public Emoji? Emoji { get; } = emoji;

        public bool? Default { get; } = @default;
    }
}
