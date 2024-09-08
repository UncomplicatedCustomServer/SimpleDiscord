using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class ThreadMember(long? id, long? userId, string? joinTimestamp, SocketMember? member)
    {
        [JsonProperty("id")]
        public long? Id { get; } = id;

        [JsonProperty("user_id")]
        public long? UserId { get; } = userId;

        [JsonProperty("join_timestamp")]
        public string? JoinTimestamp { get; } = joinTimestamp;

        [JsonProperty("member")]
        public SocketMember? Member { get; internal set; } = member;
    }
}
