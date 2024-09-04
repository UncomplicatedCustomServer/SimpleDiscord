using Newtonsoft.Json;
using SimpleDiscord.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MessageCreateMember(long? guild_id, long id, long channelId, SocketUser author, string content, string timestamp, string editedTimestamp, bool tts, bool mentionEveryone, SocketUser[] mentions, Role[] mentionRoles, Attachment[] attachments, Embed[] embeds, Reaction[] reactions, bool pinned, long? webhookId, int type, int? flags, MessageReference? messageReference, SocketPartialChannel[]? threads, Poll? poll) : SocketMessage(id, channelId, author, content, timestamp, editedTimestamp, tts, mentionEveryone, mentions, mentionRoles, attachments, embeds, reactions, pinned, webhookId, type, flags, messageReference, threads, poll)
    {
        //TODO: member, mentions
        [JsonProperty("guild_id")]
        public long? GuildId { get; } = guild_id;
    }
}
