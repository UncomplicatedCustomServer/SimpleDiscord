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

        public Guild? Guild { get; }

        public long? TargetId { get; }

        public ApplicationCommandInteractionData(long id, string name, int type, ResolvedData? resolved, ReplyCommandOption[]? options, long? guildId, long? targetId)
        {
            Id = id;
            Name = name;
            Type = type;
            Resolved = resolved;
            Options = options;
            TargetId = targetId;

            Guild = null;
            if (guildId is not null)
                Guild = Guild.GetSafeGuild((long)guildId);
        }

        public ApplicationCommandInteractionData(ApplicationCommandInteractionData self) : this(self.Id, self.Name, self.Type, self.Resolved, self.Options, self.Guild?.Id, self.TargetId)
        { }

        public ApplicationCommandInteractionData(SocketApplicationCommandInteractionData socketInstance) : this(socketInstance.Id, socketInstance.Name, socketInstance.Type, socketInstance.Resolved is not null ? new(socketInstance.Resolved) : null, socketInstance.Options, socketInstance.GuildId, socketInstance.TargetId) 
        { }
    }
}
