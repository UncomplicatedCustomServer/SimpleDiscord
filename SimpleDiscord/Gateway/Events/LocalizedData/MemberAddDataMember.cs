using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class MemberAddDataMember(SocketMember member, long guildId)
    {
        public SocketMember Member { get; } = member;

        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;
    }
}
