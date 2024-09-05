namespace SimpleDiscord.Components.Builders
{
    public class ApplicationCommandBuilder(string name, string description)
    {
        private readonly SocketSendApplicationCommand ApplicationCommand = new(name, description);

        public ApplicationCommandBuilder IsNsfw(bool isNsfw = false)
        {
            ApplicationCommand.Nsfw = isNsfw;
            return this;
        }

        public ApplicationCommandBuilder SetLocalizedName(string lan, string name)
        {
            ApplicationCommand.NameLocalizations ??= [];
            ApplicationCommand.NameLocalizations.Add(lan, name);
            return this;
        }

        public ApplicationCommandBuilder SetLocalizedDescription(string lan, string desc)
        {
            ApplicationCommand.DescriptionLocalizations ??= [];
            ApplicationCommand.DescriptionLocalizations.Add(lan, desc);
            return this;
        }

        public ApplicationCommandBuilder AddOption(SocketCommandOption option)
        {
            ApplicationCommand.Options ??= [];
            ApplicationCommand.Options.Add(option);
            return this;
        }

        public static ApplicationCommandBuilder New(string name, string desc) => new(name, desc);

        public static implicit operator SocketSendApplicationCommand(ApplicationCommandBuilder builder) => builder.ApplicationCommand;
    }
}
