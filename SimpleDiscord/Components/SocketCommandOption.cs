using Newtonsoft.Json;
using SimpleDiscord.Enums;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketCommandOption
    {
        public int Type { get; internal set; }

        public string Name { get; internal set; }

        [JsonProperty("name_localizations")]
        public Dictionary<string, string>? NameLocalizations { get; internal set; }

        public string Description { get; internal set; }

        [JsonProperty("description_localizations")]
        public Dictionary<string, string>? DescriptionLocalizations { get; internal set; }

        public bool? Required { get; internal set; }

        public List<CommandOptionChoice>? Choiches { get; internal set; }

        public List<SocketCommandOption>? Options { get; internal set; }

        [JsonProperty("min_length")]
        public int? MinLength { get; internal set; }

        [JsonProperty("max_length")]
        public int? MaxLength { get; internal set; }

        public bool Autocomplete => false;

        [JsonConstructor]
        public SocketCommandOption(int type, string name, Dictionary<string, string>? nameLocalizations, string description, Dictionary<string, string>? descriptionLocalizations, bool? required, List<CommandOptionChoice>? choiches, List<SocketCommandOption>? options, int? minLength, int? maxLength)
        {
            Type = type;
            Name = name;
            NameLocalizations = nameLocalizations;
            Description = description;
            DescriptionLocalizations = descriptionLocalizations;
            Required = required;
            Choiches = choiches;
            Options = options;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public SocketCommandOption(string name, CommandOptionType type, string description = "")
        {
            Name = name;
            Type = (int)type;
            Description = description;
        }
    }
}
