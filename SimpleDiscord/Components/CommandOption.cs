namespace SimpleDiscord.Components
{
#nullable enable
    public class CommandOption(string name, int type, object? value, CommandOption[]? options, bool? focused)
    {
        public string Name { get; } = name;

        public int Type { get; } = type;

        public object? Value { get; } = value;

        public CommandOption[]? Options { get; } = options;

        public bool? Focused { get; } = focused;
    }
}
