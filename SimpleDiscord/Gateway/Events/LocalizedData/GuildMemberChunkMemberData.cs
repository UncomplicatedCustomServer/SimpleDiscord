using Newtonsoft.Json;
using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class GuildMemberChunkMemberData(long guildId, SocketMember[] members, int chunkIndex, int chunkCount)
    {
        [JsonProperty("guild_id")]
        public long GuildId { get; } = guildId;

        public SocketMember[] Members { get; } = members;

        [JsonProperty("chunk_index")]
        public int ChunkIndex { get; } = chunkIndex;

        [JsonProperty("chunk_count")]
        public int ChunkCount { get; } = chunkCount;
    }
}
