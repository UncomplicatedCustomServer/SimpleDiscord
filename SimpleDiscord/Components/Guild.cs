using SimpleDiscord.Components.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuild))]
    public class Guild : SocketGuild
    {
        public static readonly List<Guild> List = [];

        public DateTimeOffset JoinedAt { get; }

        public bool Large { get; }

        public int MemberCount { get; }

        public SocketMember[] Members { get; }

        public List<GuildChannel> Channels { get; }

        public List<GuildThreadChannel> Threads { get; }

        public SocketPresence[] Presences { get; }

        public SocketStageInstance[] StageInstances { get; }

        public SocketScheduledEvent[] GuildScheduledEvents { get; }

        public Guild(DateTimeOffset joinedAt, bool large, int memberCount, SocketMember[] members, GuildChannel[] channels, GuildThreadChannel[] threads, SocketPresence[] presences, SocketStageInstance[] stageInstances, SocketScheduledEvent[] guildScheduledEvents, SocketGuild guild) : base(guild)
        {
            JoinedAt = joinedAt;
            Large = large;
            MemberCount = memberCount;
            Members = members;
            Channels = [.. channels];
            Threads = [.. threads];
            Presences = presences;
            StageInstances = stageInstances;
            GuildScheduledEvents = guildScheduledEvents;

            Guild instance = List.FirstOrDefault(c => c.Id == Id);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }

        public Guild(AnonymousGuild anonymous) : base(anonymous)
        {
            Console.WriteLine(anonymous.JoinedAt);
            if (anonymous.JoinedAt is not null && !true)
                JoinedAt = DateTimeOffset.Parse(anonymous.JoinedAt);
            Large = anonymous.Large;
            MemberCount = anonymous.MemberCount;
            Members = anonymous.Members;
            Channels = [];
            Threads = [];
            Presences = anonymous.Presences;
            StageInstances = anonymous.StageInstances;
            GuildScheduledEvents = anonymous.GuildScheduledEvents;

            Guild instance = List.FirstOrDefault(c => c.Id == Id);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }

        public GuildChannel? GetChannel(long id) => Channels.FirstOrDefault(channel => channel.Id == id);

        public SocketMember GetMember(long id) => Members.FirstOrDefault(member => member.User is not null && member.User.Id == id);

        internal void SafeUpdateChannel(GuildChannel channel)
        {
            Console.WriteLine($"Calling update for channel {channel.Name}\n");
            if (channel is GuildThreadChannel thread)
            {
                SafeUpdateThread(thread);
                return;
            }

            GuildChannel instance = Channels.FirstOrDefault(c => c.Id == channel.Id && c.GuildId == Id);
            if (instance is not null)
                Channels[Channels.IndexOf(instance)] = channel;
            else
                Channels.Add(channel);
        }

        internal void SafeUpdateThread(GuildThreadChannel thread)
        {
            GuildThreadChannel instance = Threads.FirstOrDefault(c => c.Id == thread.Id && c.GuildId == Id);
            if (instance is not null)
                Threads[Threads.IndexOf(instance)] = thread;
            else
                Threads.Add(thread);
        }
    }
}
