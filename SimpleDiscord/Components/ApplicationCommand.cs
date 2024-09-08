using SimpleDiscord.Enums;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    public class ApplicationCommand : SocketApplicationCommand
    {
        public new CommandType Type => CommandType.CHAT_INPUT;

        public Guild? Guild { get; } = null;

        public bool Global => Guild is null;

        public ApplicationCommand(SocketApplicationCommand command) : base(command)
        {
            if (command.GuildId is not null)
                Guild = Guild.GetSafeGuild((long)command.GuildId);
        }

        public Task<SocketApplicationCommand>? Update(SocketSendApplicationCommand command) => Edit(command);

        public Task<SocketApplicationCommand>? Edit(SocketSendApplicationCommand command)
        {
            if (Guild is not null)
                return Guild.Client.RestHttp.EditGuildCommand(Guild, this, command);
            else
                return null; // TIP: If you want to edit a GLOBAL command you have to do that from the Client class! cheers
        }

        public void Remove() => Delete();

        public void Delete() => Guild?.Client.RestHttp.DeleteGuildCommand(Guild, this); // ^ Yeah also if you want to remove it
    }
}
