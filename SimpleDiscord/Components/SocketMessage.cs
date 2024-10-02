using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Components.DiscordComponents;

namespace SimpleDiscord.Components
{
#nullable enable
    [EndpointInfo("/channels/{channel.id}/messages/{message.id}", "MESSAGE")]
    public class SocketMessage : ClientChild
    {
        public long Id { get; }

        [JsonProperty("channel_id")]
        public long ChannelId { get; }

        [JsonProperty("guild_id")]
        public long? GuildId { get; }

        public SocketUser Author { get; }

        public string Content { get; }

        public string Timestamp { get; }

        [JsonProperty("edited_timestamp")]
        public string EditedTimestamp { get; }

        public bool Tts { get; }

        [JsonProperty("mention_everyone")]
        public bool MentionEveryone { get; }

        public SocketUser[] Mentions { get; }

        [JsonProperty("mention_roles")]
        public Role[] MentionRoles { get; }

        public Attachment[] Attachments { get; }

        public Embed[] Embeds { get; }

        public SocketPartialReaction[] Reactions { get; }

        public bool Pinned { get; }

        [JsonProperty("webhook_id")]
        public long? WebhookId { get; }

        public int Type { get; }

        public int? Flags { get; }

        public object[]? Components { get; }

        [JsonProperty("message_reference")]
        public MessageReference? MessageReference { get; }

        public SocketGuildThreadChannel? Thread { get; }

        public SocketPoll? Poll { get; }

        [JsonConstructor]
        public SocketMessage(long id, long channelId, long? guildId, SocketUser author, string content, string timestamp, string editedTimestamp, bool tts, bool mentionEveryone, SocketUser[] mentions, Role[] mentionRoles, Attachment[] attachments, Embed[] embeds, SocketPartialReaction[] reactions, bool pinned, long? webhookId, int type, int? flags, object[] components, MessageReference? messageReference, SocketGuildThreadChannel? thread, SocketPoll? poll)
        {
            Id = id;
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
            Components = components;
            MessageReference = messageReference;
            Thread = thread;
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
            Components = instance.Components;
            MessageReference = instance.MessageReference;
            Thread = instance.Thread;
            Poll = instance.Poll;
        }
    }
}
