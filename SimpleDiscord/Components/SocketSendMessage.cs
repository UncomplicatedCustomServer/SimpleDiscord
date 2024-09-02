namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendMessage(string? content, bool? tts = null, Embed[]? embeds = null, string? allowedMentions = null, MessageReference? messageReference = null, Attachment[]? attachments = null, int? flags = null, Poll? poll = null)
    {
        public string? Content { get; } = content;

        public bool? Tts { get; } = tts;

        public Embed[]? Embeds { get; } = embeds;

        public string? AllowedMentions { get; } = allowedMentions;

        public MessageReference? MessageReference { get; } = messageReference;

        public Attachment[]? Attachments { get; } = attachments;

        public int? Flags { get; } = flags;

        public Poll? Poll { get; } = poll;
    }
}
