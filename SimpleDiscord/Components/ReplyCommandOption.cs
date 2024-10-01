using System;

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

        public object? GetValue()
        {
            Type? type = Type switch
            {
                3 => typeof(string),
                4 => typeof(int),
                5 => typeof(bool),
                10 => typeof(float),
                6 or 7 or 8 or 9 => typeof(long),
                _ => null
            };

            if (type is null)
                return Value;
            
            return Convert.ChangeType(Value, type);
        }
    }
}
