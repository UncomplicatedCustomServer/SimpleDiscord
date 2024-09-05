using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuildTextChannel))]
    public class GuildTextChannel : GuildChannel, IGuildElement
    {
        public string? Topic { get; }

        public bool? Nsfw { get; }

        public long? LastMessageId { get; }

        public int? RateLimitPerUser { get; }

        public int? TotalMessageSent { get; }

        public List<Message> Messages { get; } = [];

        [JsonConstructor]
        public GuildTextChannel(long id, int type, long guildId, int? position, Overwrite[]? permissionOverwrites, string? name, long? parentId, string? permissions, int? flags, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, int? totalMessageSent, bool safeUpdate = true) : base(id, type, guildId, position, permissionOverwrites, name, parentId, permissions, flags, false)
        {
            Topic = topic;
            Nsfw = nsfw;
            LastMessageId = lastMessageId;
            RateLimitPerUser = rateLimitPerUser;
            TotalMessageSent = totalMessageSent;

            if (safeUpdate)
                Guild.SafeUpdateChannel(this);
        }

        public GuildTextChannel(GuildChannel baseChannel, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, int? totalMessageSent) : base(baseChannel)
        {
            Topic = topic;
            Nsfw = nsfw;
            LastMessageId = lastMessageId;
            RateLimitPerUser = rateLimitPerUser;
            TotalMessageSent = totalMessageSent;
        }

        public GuildTextChannel(GuildTextChannel baseChannel) : this(baseChannel, baseChannel.Topic, baseChannel.Nsfw, baseChannel.LastMessageId, baseChannel.RateLimitPerUser, baseChannel.TotalMessageSent)
        { }

        public GuildTextChannel(SocketGuildTextChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            Topic = socketChannel.Topic;
            Nsfw = socketChannel.Nsfw;
            LastMessageId = socketChannel.LastMessageId;
            RateLimitPerUser = socketChannel.RateLimitPerUser;
            TotalMessageSent = socketChannel.TotalMessageSent;

            if (pushUpdate)
                Guild.SafeUpdateChannel(this);
        }

        internal void SafeUpdateMessage(Message message)
        {
            if (!Guild.Client.Config.SaveMessages)
                return;

            Message instance = Messages.FirstOrDefault(msg => msg.Id == message.Id);
            if (instance is null)
                Messages.Add(message);
            else
                Messages[Messages.IndexOf(instance)] = message;
        }
        
        internal Message GetSafeMessage(long id) => Messages.FirstOrDefault(msg => msg.Id == id);

        public async Task<SocketMessage> SendMessage(string content) => await Guild.Client.RestHttp.SendMessage(this, new(content));

        public async Task<SocketMessage> SendMessage(SocketSendMessage msg) => await Guild.Client.RestHttp.SendMessage(this, msg);

    }
}
