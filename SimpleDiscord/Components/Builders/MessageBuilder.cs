using SimpleDiscord.Components.DiscordComponents;
using SimpleDiscord.Enums;
using System.Collections.Generic;
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
                Message.Embeds.Add(embed);

            return this;
        }

        public MessageBuilder SetEmbeds(List<Embed> embeds)
        {
            Message.Embeds = embeds;
            return this;
        }

        public MessageBuilder AddComponent(ActionRow actionRow)
        {
            if (Message.Components is null)
                Message.Components = [actionRow];
            else
                Message.Components.Add(actionRow);

            return this;
        }

        public MessageBuilder SetComponents(List<SocketActionRow> actionRows)
        {
            Message.Components = actionRows;
            return this;
        }

        public MessageBuilder AddAttachment(Attachment attachment)
        {
            if (Message.Attachments is null)
                Message.Attachments = [attachment];
            else
                Message.Attachments.Add(attachment);

            return this;
        }

        public MessageBuilder SetAttachments(List<Attachment> attachments)
        {
            Message.Attachments = attachments;
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

        public MessageBuilder SetPoll(SocketSendPoll poll)
        {
            Message.Content = null;
            Message.Poll = poll;
            return this;
        }

        public static MessageBuilder New() => new();

        public static implicit operator SocketSendMessage(MessageBuilder builder) => builder.Message;
    }
}
