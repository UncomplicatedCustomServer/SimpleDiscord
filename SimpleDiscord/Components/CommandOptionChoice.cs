using System.Collections.Generic;

namespace SimpleDiscord.Components
{
    public class CommandOptionChoice(string name, Dictionary<string, string> nameLocalizations, object value)
    {
        public string Name { get; } = name;

        public Dictionary<string, string> NameLocalizations { get; } = nameLocalizations;

        public object Value { get; } = value;
    }
}
