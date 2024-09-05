using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketApplicationCommandInteractionData : SocketInteractionData
    {
        public long Id { get; }

        public string Name { get; }

        public int Type { get; }

        public SocketResolvedData? Data { get; }

        public ReplyCommandOption[]? Options { get; }

        public long? GuildId { get; }

        public long? TargetId { get; }

        [JsonConstructor]
        public SocketApplicationCommandInteractionData(long id, string name, int type, SocketResolvedData? data, ReplyCommandOption[]? options, long? guildId, long? targetId)
        {
            Id = id;
            Name = name;
            Type = type;
            Data = data;
            Options = options;
            GuildId = guildId;
            TargetId = targetId;
        }

        public SocketApplicationCommandInteractionData(SocketApplicationCommandInteractionData self)
        {
            Id = self.Id;
            Name = self.Name;
            Type = self.Type;
            Data = self.Data;
            Options = self.Options;
            GuildId = self.GuildId;
            TargetId = self.TargetId;
        }
    }
}
