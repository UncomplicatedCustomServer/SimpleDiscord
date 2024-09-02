using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketMessage
    {
        public static HashSet<SocketMessage> List { get; } = new();

        public long Id { get; }

        public long ChannelId { get; }

        public SocketUser Author { get; }

        public string Content { get; }

        public string Timestamp { get; }

        public string EditedTimestamp { get; }

        public bool Tts { get; }

        public bool MentionEveryone { get; }

        public SocketUser[] Mentions { get; }

        public Role[] MentionRoles { get; }

        public Attachment[] Attachments { get; }

        public Embed[] Embeds { get; }

        public Reaction[] Reactions { get; }

        public bool Pinned { get; }

        public long? WebhookId { get; }

        public int Type { get; }

        public int? Flags { get; }

        public MessageReference? MessageReference { get; }

        public SocketPartialChannel[]? Threads { get; }

        public Poll? Poll { get; }

        public SocketMessage(long id, long channelId, SocketUser author, string content, string timestamp, string editedTimestamp, bool tts, bool mentionEveryone, SocketUser[] mentions, Role[] mentionRoles, Attachment[] attachments, Embed[] embeds, Reaction[] reactions, bool pinned, long? webhookId, int type, int? flags, MessageReference? messageReference, SocketPartialChannel[]? threads, Poll? poll)
        {
            Id = id;
            ChannelId = channelId;
            Author = author;
            Content = content;
            Timestamp = timestamp;
            EditedTimestamp = editedTimestamp;
            Tts = tts;
            MentionEveryone = mentionEveryone;
            Mentions = mentions;
            MentionRoles = mentionRoles;
            Attachments = attachments;
            Embeds = embeds;
            Reactions = reactions;
            Pinned = pinned;
            WebhookId = webhookId;
            Type = type;
            Flags = flags;
            MessageReference = messageReference;
            Threads = threads;
            Poll = poll;

            if (!List.Any(msg => msg.Id == Id))
                List.Add(this);
        }
    }
}
