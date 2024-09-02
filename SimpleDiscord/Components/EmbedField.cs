namespace SimpleDiscord.Components
{
    public class EmbedField(string name, string value, bool? inline = true)
    {
        public string Name { get; } = name;

        public string Value { get; } = value;

        public bool? Inline { get; } = inline;
    }
}
