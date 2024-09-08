using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class CommandOptionChoice(string name, object value, Dictionary<string, string>? nameLocalizations = null)
    {
        public string Name { get; } = name;

        public Dictionary<string, string>? NameLocalizations { get; } = nameLocalizations;

        public object Value { get; } = value;
    }
}
