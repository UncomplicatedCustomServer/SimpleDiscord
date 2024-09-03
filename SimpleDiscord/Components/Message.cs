using SimpleDiscord.Components.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace SimpleDiscord.Components
{
    [SocketInstance(typeof(SocketMessage))]
    public class Message : SocketMessage
    {
        public static readonly List<Message> List = [];

        public GuildTextChannel Channel { get; }

        public SocketMember MemberAuthor { get; }

        public Guild Guild { get; }

        public new GuildThreadChannel[] Threads { get; }

        public Message(SocketMessage baseMessage) : base(baseMessage)
        {
            Console.WriteLine($"Called message ..ctor with {baseMessage.Content} - {baseMessage.Id}\n\n");
            List<GuildThreadChannel> _threads = [];
            if (baseMessage.Threads is not null)
                foreach (SocketGuildThreadChannel socketChannel in baseMessage.Threads)
                    _threads.Add(new(socketChannel));

            Threads = [.. _threads];
            Console.WriteLine($"We have a total of {GuildChannel.List.Count} cached channels!");
            Channel = GuildChannel.List.FirstOrDefault(channel => channel.Id == base.ChannelId) as GuildTextChannel;
            if (Channel is null)
                Console.WriteLine($"Somehow channel from {base.ChannelId} is null!");

            Guild = Channel.Guild;
            MemberAuthor = Guild.GetMember(baseMessage.Author.Id);
        }
    }
}
