using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Networking;
using System;

namespace SimpleDiscord.Components
{
#nullable enable
    [EndpointInfo("/channels/{channel.id}/messages/{message.id}", "MESSAGE")]
    public class SocketMessage : SyncableElement
    {
        public long Id { get; }

        [JsonProperty("channel_id")]
        public long ChannelId { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

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

        public SocketGuildThreadChannel[]? Threads { get; }

        public Poll? Poll { get; }

        [JsonConstructor]
        public SocketMessage(long id, long channelId, long? guildId, SocketUser author, string content, string timestamp, string editedTimestamp, bool tts, bool mentionEveryone, SocketUser[] mentions, Role[] mentionRoles, Attachment[] attachments, Embed[] embeds, Reaction[] reactions, bool pinned, long? webhookId, int type, int? flags, MessageReference? messageReference, SocketGuildThreadChannel[]? threads, Poll? poll)
        {
            Id = id;
            Console.WriteLine($"\n\nCalled SocketMessage::...ctor, with {channelId} and {guildId}!\n\n");
            ChannelId = channelId;
            GuildId = guildId;
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
        }

        public SocketMessage(SocketMessage instance)
        {
            Id = instance.Id;
            ChannelId = instance.ChannelId;
            GuildId = instance.GuildId;
            Author = instance.Author;
            Content = instance.Content;
            Timestamp = instance.Timestamp;
            EditedTimestamp = instance.EditedTimestamp;
            Tts = instance.Tts;
            MentionEveryone= instance.MentionEveryone;
            Mentions = instance.Mentions;
            MentionRoles = instance.MentionRoles;
            Attachments = instance.Attachments;
            Embeds = instance.Embeds;
            Reactions = instance.Reactions;
            Pinned = instance.Pinned;
            WebhookId= instance.WebhookId;
            Type = instance.Type;
            Flags = instance.Flags;
            MessageReference = instance.MessageReference;
            Threads = instance.Threads;
            Poll = instance.Poll;
        }
    }
}
