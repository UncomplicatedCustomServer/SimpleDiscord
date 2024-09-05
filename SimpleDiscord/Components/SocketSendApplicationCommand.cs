using Newtonsoft.Json;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendApplicationCommand
    {
        public int Type => 1;

        public string Name { get; }

        public Dictionary<string, string>? NameLocalizations { get; internal set; }

        public string Description { get; internal set; }

        public Dictionary<string, string>? DescriptionLocalizations { get; internal set; }

        public List<SocketCommandOption>? Options { get; internal set; }

        public int[]? Contexts => null;

        public bool? Nsfw { get; internal set; }

        public bool Autocomplete => false;

        [JsonConstructor]
        public SocketSendApplicationCommand(string name, Dictionary<string, string>? nameLocalizations, string description, Dictionary<string, string>? descriptionLocalizations, SocketCommandOption[] options, bool? nsfw)
        {
            Name = name;
            NameLocalizations = nameLocalizations;
            Description = description;
            DescriptionLocalizations = descriptionLocalizations;
            Options = [..options];
            Nsfw = nsfw;
        }

        public SocketSendApplicationCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
