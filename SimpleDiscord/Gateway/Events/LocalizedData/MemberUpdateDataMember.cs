using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MemberUpdateDataMember(SocketUser user, string nick, string avatar, long[] roles, string joinedAt, string premiumSince, bool deaf, bool mute, int flags, bool? pending, string permissions, string communicationDisabledUntil, long guildId) : SocketMember(user, nick, avatar, roles, joinedAt, premiumSince, deaf, mute, flags, pending, permissions, communicationDisabledUntil)
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;
    }
}
