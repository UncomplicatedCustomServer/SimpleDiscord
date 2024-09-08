using SimpleDiscord.Enums;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class CommandOptionBuilder(string name, CommandOptionType type, string desc)
    {
        private readonly SocketCommandOption Option = new(name, type, desc);

        public CommandOptionBuilder AddChoice(CommandOptionChoice choice)
        {
            Option.Choiches ??= [];
            Option.Choiches.Append(choice);
            return this;
        }

        public CommandOptionBuilder AddOption(SocketCommandOption option)
        {
            Option.Options ??= [];
            Option.Options.Append(option);
            return this;
        }

        public CommandOptionBuilder AddChoice(string name, object value)
        {
            AddChoice(new(name, value));
            return this;
        }

        public CommandOptionBuilder AddOption(string name, CommandOptionType type, string desc)
        {
            AddOption(new(name, type, desc));
            return this;
        }

        public CommandOptionBuilder MaxLength(int max)
        {
            Option.MaxLength = max;
            return this;
        }

        public CommandOptionBuilder MinLength(int min)
        {
            Option.MinLength = min;
            return this;
        }

        public CommandOptionBuilder AddLocalizedName(string lan, string name)
        {
            Option.NameLocalizations ??= [];
            Option.NameLocalizations.Add(lan, name);
            return this;
        }

        public CommandOptionBuilder AddLocalizedDescription(string lan, string desc)
        {
            Option.DescriptionLocalizations ??= [];
            Option.DescriptionLocalizations.Add(lan, desc);
            return this;
        }

        public CommandOptionBuilder SetRequired(bool required = true)
        {
            Option.Required = required;
            return this;
        }

        public static CommandOptionBuilder New(string name, CommandOptionType type, string desc) => new(name, type, desc);

        public static implicit operator SocketCommandOption(CommandOptionBuilder builder) => builder.Option;
    }
}
