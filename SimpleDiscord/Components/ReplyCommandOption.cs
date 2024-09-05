namespace SimpleDiscord.Components
{
#nullable enable
    public class ReplyCommandOption(string name, int type, object? value, ReplyCommandOption[]? options, bool? focused)
    {
        public string Name { get; } = name;

        public int Type { get; } = type;

        public object? Value { get; } = value;

        public ReplyCommandOption[]? Options { get; } = options;

        public bool? Focused { get; } = focused;
    }
}
