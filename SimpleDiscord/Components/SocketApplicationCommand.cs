using Newtonsoft.Json;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketApplicationCommand
    {
        public long Id { get; }

        public int Type { get; }

        [JsonProperty("application_id")]
        public long ApplicationId { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

        public string Name { get; }

        [JsonProperty("name_localizations")]
        public Dictionary<string, string>? NameLocalizations { get; }

        public string Description { get; }

        [JsonProperty("description_localizations")]
        public Dictionary<string, string>? DescriptionLocalizations { get; }

        public SocketCommandOption[]? Options { get; }

        [JsonProperty("dm_permission")]
        public bool? DmPermission { get; }

        public bool? Nsfw { get; }

        public long Version { get; }

        [JsonConstructor]
        public SocketApplicationCommand(long id, int type, long applicationId, long? guildId, string name, Dictionary<string, string>? nameLocalizations, string description, Dictionary<string, string>? descriptionLocalizations, SocketCommandOption[]? options, bool? dmPermission, bool? nsfw, long version)
        {
            Id = id;
            Type = type;
            ApplicationId = applicationId;
            GuildId = guildId;
            Name = name;
            NameLocalizations = nameLocalizations;
            Description = description;
            DescriptionLocalizations = descriptionLocalizations;
            Options = options;
            DmPermission = dmPermission;
            Nsfw = nsfw;
            Version = version;
        }

        public SocketApplicationCommand(SocketApplicationCommand instance)
        {
            Id = instance.Id;
            Type = instance.Type;
            ApplicationId = instance.ApplicationId;
            GuildId = instance.GuildId;
            Name = instance.Name;
            NameLocalizations = instance.NameLocalizations;
            Description = instance.Description;
            DescriptionLocalizations = instance.DescriptionLocalizations;
            Options = instance.Options;
            DmPermission = instance.DmPermission;
            Nsfw = instance.Nsfw;
            Version = instance.Version;
        }
    }
}
