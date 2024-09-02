namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildVoiceChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, string topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, long? parentId, int? messageCount, int memberCount, string permissions, int? flags, int? totalMessageSent) : SocketGuildTextChannel(id, type, guildId, position, permissionOverwrites, name, topic, nsfw, lastMessageId, rateLimitPerUser, parentId, messageCount, memberCount, permissions, flags, totalMessageSent)
    {
        public int Bitrate { get; }

        public int UserLimit { get; }

        public string? RtcRegion { get; }

        public int? VideoQualityMode { get; }
    }
}
