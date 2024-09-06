using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
#nullable enable
    public class ThreadMemberUpdateMember(long id, long guildId, int memberCount, ThreadMember[] addedMembers, long[]? removedMemberIds)
    {
        public long Id { get; } = id;

        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;

        [JsonProperty("member_count")]
        public int MemberCount { get; } = memberCount;

        [JsonProperty("added_members")]
        public ThreadMember[] AddedMembers { get; } = addedMembers;

        [JsonProperty("removed_member_ids")]
        public long[]? RemovedMemberIds { get; } = removedMemberIds;
    }
}
