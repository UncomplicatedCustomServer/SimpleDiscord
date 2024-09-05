using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
    public class ApplicationCommand : SocketApplicationCommand
    {
        public new CommandType Type => CommandType.CHAT_INPUT;

        public Guild Guild { get; } = null;

        public bool Global => Guild is null;

        public ApplicationCommand(SocketApplicationCommand command) : base(command)
        {
            if (command.GuildId is not null)
                Guild = Guild.GetSafeGuild((long)command.GuildId);
        }
    }
}
