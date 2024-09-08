using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketApplicationCommandInteractionData))]
    public class ApplicationCommandInteractionData : InteractionData
    {
        public long Id { get; }

        public string Name { get; }

        public int Type { get; }

        public ResolvedData? Resolved { get; }

        public ReplyCommandOption[]? Options { get; }

        public long? TargetId { get; }

        [JsonConstructor]
        public ApplicationCommandInteractionData(long id, string name, int type, ResolvedData? resolved, ReplyCommandOption[]? options, long? targetId)
        {
            Id = id;
            Name = name;
            Type = type;
            Resolved = resolved;
            Options = options;
            TargetId = targetId;
        }

        public ApplicationCommandInteractionData(ApplicationCommandInteractionData self) : this(self.Id, self.Name, self.Type, self.Resolved, self.Options, self.TargetId)
        { }

        public ApplicationCommandInteractionData(SocketApplicationCommandInteractionData socketInstance) : this(socketInstance.Id, socketInstance.Name, socketInstance.Type, socketInstance.Resolved is not null ? new(socketInstance.Resolved) : null, socketInstance.Options, socketInstance.TargetId) 
        { }
    }
}
