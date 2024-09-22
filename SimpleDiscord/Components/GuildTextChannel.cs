using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Logger;
using System;
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

        public List<GuildThreadChannel> Threads { get; } = [];

        public string Link { get; }

        public GuildTextChannel(SocketGuildTextChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            Topic = socketChannel.Topic;
            Nsfw = socketChannel.Nsfw;
            LastMessageId = socketChannel.LastMessageId;
            RateLimitPerUser = socketChannel.RateLimitPerUser;
            TotalMessageSent = socketChannel.TotalMessageSent;

            Link = $"https://discord.com/channels/{GuildId}/{Id}";

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

        internal void SafeUpdateThread(GuildThreadChannel thread)
        {
            GuildThreadChannel instance = Threads.FirstOrDefault(t => t.Id == thread.Id);
            if (instance is null)
                Threads.Add(thread);
            else
                Threads[Threads.IndexOf(instance)] = thread;
        }

        internal void SafeClearMessage(long id)
        {
            Message instance = Messages.FirstOrDefault(msg => msg.Id == id);
            if (instance is not null)
                Messages.Remove(instance);
        }

        internal void SafeClearThread(long id)
        {
            GuildThreadChannel instance = Threads.FirstOrDefault(t => t.Id == id);
            instance?.Dispose();
            if (instance is not null)
                Threads.Remove(instance);
        }

        internal override void Dispose()
        {
            foreach (GuildThreadChannel thread in Threads)
                thread.Dispose();
            base.Dispose();
        }

        internal Message GetSafeMessage(long id) => Messages.FirstOrDefault(msg => msg.Id == id);

        public async Task<SocketMessage> SendMessage(string content) => await SendMessage(new SocketSendMessage(content));

        public async Task<SocketMessage> SendMessage(SocketSendMessage msg) => await Guild.Client.RestHttp.SendMessage(this, msg);

        public async Task<Message> GetMessage(long id) => await Guild.Client.RestHttp.GetMessage(this, id);

        public async Task<SocketMessage[]> GetMessages(int? limit = 50, long? around = null, long? before = null, long? after = null) => await Guild.Client.RestHttp.GetMessages(this, limit, around, before, after);
    }
}
