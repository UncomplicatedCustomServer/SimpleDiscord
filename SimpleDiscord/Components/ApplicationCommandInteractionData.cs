using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;
using System.Linq;

namespace SimpleDiscord.Components
{
#pragma warning disable IDE0290
#nullable enable
    [SocketInstance(typeof(SocketApplicationCommandInteractionData))]
    public class ApplicationCommandInteractionData : InteractionData
    {
        public long Id { get; }

        public string Name { get; }

        public int Type { get; }

        public ResolvedData? Resolved { get; }

        public ReplyCommandOption[]? Options { get; internal set; }

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

        public ReplyCommandOption? GetOption(string name) => Options?.FirstOrDefault(o => o.Name == name);

        public bool TryGetOption(string name, out ReplyCommandOption? option)
        {
            option = GetOption(name);
            return option is not null;
        }
    }
}
