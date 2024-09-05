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

        public ResolvedData? Data { get; }

        public CommandOption[]? Options { get; }

        public Guild? Guild { get; }

        public long? TargetId { get; }

        public ApplicationCommandInteractionData(long id, string name, int type, ResolvedData? data, CommandOption[]? options, long? guildId, long? targetId)
        {
            Id = id;
            Name = name;
            Type = type;
            Data = data;
            Options = options;
            TargetId = targetId;

            Guild = null;
            if (guildId is not null)
                Guild = Guild.GetSafeGuild((long)guildId);
        }

        public ApplicationCommandInteractionData(ApplicationCommandInteractionData self) : this(self.Id, self.Name, self.Type, self.Data, self.Options, self.Guild?.Id, self.TargetId)
        { }

        public ApplicationCommandInteractionData(SocketApplicationCommandInteractionData socketInstance) : this(socketInstance.Id, socketInstance.Name, socketInstance.Type, socketInstance.Data is not null ? new(socketInstance.Data) : null, socketInstance.Options, socketInstance.GuildId, socketInstance.TargetId) 
        { }
    }
}
