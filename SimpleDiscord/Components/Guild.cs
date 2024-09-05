using SimpleDiscord.Components.Attributes;
using SimpleDiscord.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public List<ApplicationCommand> Commands { get; } = [];

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

            if (Client.Config.SaveGuildRegisteredCommands)
            {
                Task<SocketApplicationCommand[]> commands = Client.RestHttp.GetGuildCommands(this);
                commands.Wait();
                foreach (SocketApplicationCommand command in commands.Result)
                    Commands.Add(new(command));
            }

            Guild instance = List.FirstOrDefault(c => c.Id == Id);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }

        private bool HasCommand(string name) => Commands.Any(c => c.Name == name);

        public GuildChannel? GetChannel(long id) => Channels.FirstOrDefault(channel => channel.Id == id);

        internal GuildChannel GetSafeChannel(long id) => Channels.FirstOrDefault(channel => channel.Id == id);

        public SocketMember GetMember(long id) => Members.FirstOrDefault(member => member.User is not null && member.User.Id == id);

        public async Task<ApplicationCommand?> RegisterCommand(SocketSendApplicationCommand command)
        {
            if (Client.Config.RegisterCommands is RegisterCommandType.None)
                return null;

            if (HasCommand(command.Name) && Client.Config.RegisterCommands is not RegisterCommandType.CreateAndEdit)
                return null;

            ApplicationCommand createdCommand = new(await Client.RestHttp.CreateGuildCommand(this, command));
            SafeUpdateCommand(createdCommand);
            return createdCommand;
        }

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

        internal void SafeUpdateCommand(ApplicationCommand cmd)
        {
            ApplicationCommand instance = Commands.FirstOrDefault(c => c.Name == cmd.Name);
            if (instance is not null)
                Commands[Commands.IndexOf(instance)] = cmd;
            else
                Commands.Add(cmd);
        }

        public static Guild? GetGuild(long id) => List.FirstOrDefault(guild => guild.Id == id);

        public static Guild GetSafeGuild(long id) => List.FirstOrDefault(guild => guild.Id == id);
    }
}
