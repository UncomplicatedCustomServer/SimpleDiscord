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

        public List<Member> Members { get; }

        public List<GuildChannel> Channels { get; }

        public List<GuildThreadChannel> Threads { get; }

        public List<Presence> Presences { get; }

        public SocketStageInstance[] StageInstances { get; }

        public SocketScheduledEvent[] GuildScheduledEvents { get; }

        public List<ApplicationCommand> Commands { get; } = [];

        public new List<Role> Roles { get; }

        public new List<Emoji> Emojis { get; private set; }

        public Guild(AnonymousGuild anonymous) : base(anonymous)
        {
            if (anonymous.JoinedAt is not null && !true)
                JoinedAt = DateTimeOffset.Parse(anonymous.JoinedAt);
            Large = anonymous.Large;
            MemberCount = anonymous.MemberCount;
            Members = [];
            Channels = [];
            Threads = [];
            Presences = [];

            StageInstances = anonymous.StageInstances;
            GuildScheduledEvents = anonymous.GuildScheduledEvents;
            Roles = [.. anonymous.Roles];
            Emojis = [.. anonymous.Emojis];

            foreach (SocketMember member in anonymous.Members)
                Members.Add(new(this, member));

            foreach (SocketPresence presence in anonymous.Presences)
                Presences.Add(new(presence, this));

            Guild instance = List.FirstOrDefault(c => c.Id == Id);
            if (instance is not null)
                List.Insert(List.IndexOf(instance), this);
            else
                List.Add(this);
        }

        private bool HasCommand(string name) => Commands.Any(c => c.Name == name);

        public GuildChannel? GetChannel(long id) => Channels.FirstOrDefault(channel => channel.Id == id);

        internal GuildChannel GetSafeChannel(long id) => Channels.FirstOrDefault(channel => channel.Id == id);

        internal void SafeRegisterCommands()
        {
            if (Client.Config.SaveGuildRegisteredCommands)
            {
                Task<SocketApplicationCommand[]> commands = Client.RestHttp.GetGuildCommands(this);
                commands.Wait();
                foreach (SocketApplicationCommand command in commands.Result)
                    Commands.Add(new(command));
            }
        }

        public Member GetMember(long id) => Members.FirstOrDefault(member => member.User is not null && member.User.Id == id);

        public Role GetRole(long id) => Roles.FirstOrDefault(role => role.Id == id);

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

        internal void SafeClearChannel(GuildChannel channel)
        {
            if (channel is GuildThreadChannel thread)
            {
                SafeClearThread(thread);
                return;
            }

            SafeClearChannel(channel.Id);
        }

        internal void SafeClearChannel(long id)
        {
            GuildChannel instance = Channels.FirstOrDefault(c => c.Id == id);
            instance?.Dispose();
            if (instance is not null)
                Channels.Remove(instance);
        }

        internal void SafeUpdateThread(GuildThreadChannel thread)
        {
            GuildThreadChannel instance = Threads.FirstOrDefault(c => c.Id == thread.Id && c.GuildId == Id);
            if (instance is not null)
                Threads[Threads.IndexOf(instance)] = thread;
            else
                Threads.Add(thread);
        }

        internal void SafeClearThread(GuildThreadChannel thread) => SafeClearThread(thread.Id);

        internal void SafeClearThread(long id)
        {
            GuildThreadChannel instance = Threads.FirstOrDefault(t => t.Id == id);
            instance?.Dispose();
            if (instance is not null)
                Threads.Remove(instance);
        }

        internal void SafeUpdateCommand(ApplicationCommand cmd)
        {
            ApplicationCommand instance = Commands.FirstOrDefault(c => c.Name == cmd.Name);
            if (instance is not null)
                Commands[Commands.IndexOf(instance)] = cmd;
            else
                Commands.Add(cmd);
        }

        internal void SafeClearCommand(ApplicationCommand cmd)
        {
            ApplicationCommand instance = Commands.FirstOrDefault(c => c.Id == cmd.Id);
            if (instance is not null)
                Commands.Remove(instance);
        }

        internal void SafeUpdateRole(Role role)
        {
            Role instance = Roles.FirstOrDefault(r => r.Id == role.Id);
            if (instance is not null)
                Roles[Roles.IndexOf(instance)] = role;
            else
                Roles.Add(role);
        }

        internal void SafeClearRole(Role role) => SafeClearRole(role.Id);

        internal void SafeClearRole(long role)
        {
            Role instance = Roles.FirstOrDefault(r => r.Id == role);
            if (instance is not null)
                Roles.Remove(instance);
        }

        internal void SafeUpdateEmoji(Emoji emoji)
        {
            Emoji instance = Emojis.FirstOrDefault(e => e.Id == emoji.Id);
            if (instance is not null)
                Emojis[Emojis.IndexOf(instance)] = emoji;
            else
                Emojis.Add(emoji);
        }

        internal void SafeClearEmoji(Emoji emoji)
        {
            Emoji instance = Emojis.FirstOrDefault(e => e.Id == emoji.Id);
            if (instance is not null)
                Emojis.Remove(instance);
        }

        internal void SafeBulkUpdateEmojis(Emoji[] emojis) => Emojis = [.. emojis];

        internal void SafeUpdateMember(Member member)
        {
            Member instance = Members.FirstOrDefault(m => m.User?.Id == member.User?.Id && m.User is not null);
            if (instance is not null)
                Members[Members.IndexOf(instance)] = member;
            else
                Members.Add(member);
        }

        internal void SafeClearMember(long member)
        {
            Member instance = Members.FirstOrDefault(m => m.User?.Id == member && m.User is not null);
            if (instance is not null)
                Members.Remove(instance);
        }

        internal override void Dispose()
        {
            List.Remove(this);

            foreach (GuildChannel channel in Channels)
                channel.Dispose();

            foreach (GuildThreadChannel threads in Threads)
                threads.Dispose();

            base.Dispose();
        }

        public Task<SocketGuildChannel> CreateChannel(SocketSendGuildChannel channel, string? reason = null) => Client.RestHttp.GuildCreateChannel(this, channel, reason);

        public Task<Role> CreateRole(SocketSendRole role, string? reason = null) => Client.RestHttp.GuildCreateRole(this, role, reason);

        public Task DeleteRole(Role role, string? reason = null) => Client.RestHttp.GuildDeleteRole(this, role, reason);

        public Task UnbanUser(long id, string? reason = null) => Client.RestHttp.MemberUnban(this, id, reason);

        public static Guild? GetGuild(long id) => List.FirstOrDefault(guild => guild.Id == id);

        public static Guild GetSafeGuild(long id) => List.FirstOrDefault(guild => guild.Id == id);

        public GuildThreadChannel? GetThreadChannel(long id) => Threads.FirstOrDefault(thread => thread.Id == id);

        public GuildThreadChannel GetSafeThreadChannel(long id) => Threads.FirstOrDefault(thread => thread.Id == id);
    }
}
