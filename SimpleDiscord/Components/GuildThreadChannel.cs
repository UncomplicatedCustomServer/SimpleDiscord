﻿using SimpleDiscord.Components.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuildThreadChannel))]
    public class GuildThreadChannel : GuildTextChannel, IGuildElement
    {
        public SocketMember? Owner { get; }

        public int? MessageCount { get; }

        public int? MemberCount { get; }

        public ThreadMetadata? ThreadMetadata { get; }

        public ThreadMember? Member { get; }

        public int? DefaultAutoArchiveDuration { get; }

        public List<ThreadMember> Members { get; } = [];

        public GuildThreadChannel(SocketGuildThreadChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            if (socketChannel.OwnerId is not null)
                Owner = Guild.GetMember((long)socketChannel.OwnerId);

            MessageCount = socketChannel.MessageCount;
            MemberCount = socketChannel.MemberCount;
            ThreadMetadata = socketChannel.ThreadMetadata;
            Member = socketChannel.Member;
            DefaultAutoArchiveDuration = socketChannel.DefaultAutoArchiveDuration;

            if (Parent is not null && Parent is GuildTextChannel textChannel) // As it should :(
                textChannel.SafeUpdateThread(this);

            if (pushUpdate)
                Guild.SafeUpdateChannel(this);
        }

        internal void SafeUpdateMember(ThreadMember member)
        {
            ThreadMember instance = Members.FirstOrDefault(m => m.Id == member.Id);
            if (instance is not null)
                Members[Members.IndexOf(instance)] = member;
            else
                Members.Add(member);
        }

        internal void SafeDisposeMember(long id)
        {
            ThreadMember instance = Members.FirstOrDefault(m => m.Id == id);
            if (instance is not null)
                Members.Remove(instance);
        }

        public void FetchMembers()
        {
            if (Guild.Client.Config.FetchThreadMembers)
            {
                Task<ThreadMember[]> members = Guild.Client.RestHttp.GetThreadMembers(this);
                members.Wait();
                foreach (ThreadMember member in members.Result)
                {
                    if (member.UserId is not null)
                        member.Member = Guild.GetMember((long)member.UserId);
                    Members.Add(member);
                }
            }
        }

        internal override void Dispose()
        {
            if (Parent is not null && Parent is GuildTextChannel textChannel)
                textChannel.SafeClearThread(Id);
        }

        public Task Join() => Guild.Client.RestHttp.JoinThread(this);

        public Task Leave() => Guild.Client.RestHttp.LeaveThread(this);

        public Task AddMember(long id) => Guild.Client.RestHttp.AddUserToThread(this, id);

        public Task RemoveMember(long id) => Guild.Client.RestHttp.RemoveUserToThread(this, id);
    }
}
