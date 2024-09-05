using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class MessageBuilder()
    {
        private SocketSendMessage Message { get; } = new();

        public MessageBuilder SetContent(string content)
        {
            Message.Content = content;
            return this;
        }

        public MessageBuilder AddEmbed(Embed embed)
        {
            if (Message.Embeds is null)
                Message.Embeds = [embed];
            else
                Message.Embeds.Append(embed);

            return this;
        }

        public MessageBuilder AddComponent(ActionRow actionRow)
        {
            if (Message.Components is null)
                Message.Components = [actionRow];
            else
                Message.Components.Append(actionRow);

            return this;
        }

        public MessageBuilder SetEphemeral()
        {
            if (Message.Flags is null)
                Message.Flags = (int)MessageFlags.EPHEMERAL;
            else
                Message.Flags = (int)((MessageFlags)Message.Flags | MessageFlags.EPHEMERAL);

            return this;
        }

        public static MessageBuilder New() => new();

        public static implicit operator SocketSendMessage(MessageBuilder builder) => builder.Message;
    }
}
