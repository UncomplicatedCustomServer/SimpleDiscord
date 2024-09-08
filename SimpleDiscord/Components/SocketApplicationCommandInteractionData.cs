using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketApplicationCommandInteractionData : SocketInteractionData
    {
        public long Id { get; }

        public string Name { get; }

        public int Type { get; }

        [JsonProperty("resolved")]
        public SocketResolvedData? Resolved { get; }

        [JsonProperty("options")]
        public ReplyCommandOption[]? Options { get; }

        [JsonProperty("target_id")]
        public long? TargetId { get; }

        [JsonConstructor]
        public SocketApplicationCommandInteractionData(long id, string name, int type, SocketResolvedData? resolved, ReplyCommandOption[]? options, long? targetId)
        {
            Id = id;
            Name = name;
            Type = type;
            Resolved = resolved;
            Options = options;
            TargetId = targetId;
        }

        public SocketApplicationCommandInteractionData(SocketApplicationCommandInteractionData self)
        {
            Id = self.Id;
            Name = self.Name;
            Type = self.Type;
            Resolved = self.Resolved;
            Options = self.Options;
            TargetId = self.TargetId;
        }
    }
}
