using Newtonsoft.Json;
using SimpleDiscord.Enums;

namespace SimpleDiscord.Components.DiscordComponents
{
#nullable enable
    public class TextInput : GenericComponent
    {
        public override int Type => (int)ComponentType.TextInput;

        public string CustomId { get; }

        public int Style { get; }

        public string Label { get; }

        public int? MinLength { get; }

        public int? MaxLength { get; }

        public bool? Required { get; }

        public string? Value { get; }

        public string? Placeholder { get; }

        [JsonConstructor]
        public TextInput(string customId, int style, string label, int? minLength, int? maxLength, bool? required, string? value, string? placeholder)
        {
            CustomId = customId;
            Style = style;
            Label = label;
            MinLength = minLength;
            MaxLength = maxLength;
            Required = required;
            Value = value;
            Placeholder = placeholder;
        }

        public TextInput(string customId, TextInputStyle style, string label, int? minLength, int? maxLength, bool? required, string? value, string? placeholder) : this(customId, (int)style, label, minLength, maxLength, required, value, placeholder)
        { }
    }
}
