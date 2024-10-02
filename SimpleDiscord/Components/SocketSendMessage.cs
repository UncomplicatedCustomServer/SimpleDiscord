using SimpleDiscord.Components.DiscordComponents;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendMessage : InteractionCallbackData
    {
        public string? Content { get; internal set; } = string.Empty;

        public List<Embed>? Embeds { get; internal set; }

        public string? AllowedMentions { get; internal set; }

        public MessageReference? MessageReference { get; internal set; }

        public List<Attachment>? Attachments { get; internal set; }

        public List<SocketActionRow>? Components { get; internal set; }

        public int? Flags { get; internal set; }

        public SocketSendPoll? Poll { get; internal set; }

        public SocketSendMessage(string? content, List<Embed>? embeds = null, string? allowedMentions = null, MessageReference? messageReference = null, List<Attachment>? attachments = null, List<SocketActionRow>? components = null, int? flags = null, SocketSendPoll? poll = null)
        {
            Content = content;
            Embeds = embeds;
            AllowedMentions = allowedMentions;
            MessageReference = messageReference;
            Attachments = attachments;
            Components = components;
            Flags = flags;
            Poll = poll;
        }

        internal SocketSendMessage()
        { }
    }
}
