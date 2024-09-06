using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendMessage : InteractionCallbackData
    {
        public string? Content { get; internal set; }

        public Embed[]? Embeds { get; internal set; }

        public string? AllowedMentions { get; internal set; }

        public MessageReference? MessageReference { get; internal set; }

        public Attachment[]? Attachments { get; internal set; }

        public SocketActionRow[]? Components { get; internal set; }

        public int? Flags { get; internal set; }

        public SocketSendPoll? Poll { get; internal set; }

        public SocketSendMessage(string? content, Embed[]? embeds = null, string? allowedMentions = null, MessageReference? messageReference = null, Attachment[]? attachments = null, SocketActionRow[]? components = null, int? flags = null, SocketSendPoll? poll = null)
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
