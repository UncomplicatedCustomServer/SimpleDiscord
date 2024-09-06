using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    public class ThreadMemberUpdateMember(long id, long guildId, int memberCount, ThreadMember[] addedMembers, long[]? removedMemberIds)
    {
        public long Id { get; } = id;

        public long GuildId { get; } = guildId;

        public int MemberCount { get; } = memberCount;

        public ThreadMember[] AddedMembers { get; } = addedMembers;

        public long[]? RemovedMemberIds { get; } = removedMemberIds;
    }
}
