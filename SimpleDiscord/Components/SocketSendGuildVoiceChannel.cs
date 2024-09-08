using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketSendGuildVoiceChannel(string name, int? position = null, Overwrite[]? permissionOverwrites = null, string? topic = null, int? rateLimitPerUser = null, long? parentId = null, bool? nsfw = null, int? defaultAltoArchiveDuration = null, int? defaultThreadRateLimitPerUser = null, int? bitrate = null, int? userLimit = null, string? rtcRegion = null, int? videoQualityMode = null) : SocketSendGuildTextChannel(name, position, permissionOverwrites, topic, rateLimitPerUser, parentId, nsfw, defaultAltoArchiveDuration, defaultThreadRateLimitPerUser)
    {
        public new int Type => (int)ChannelType.GUILD_VOICE;

        public int? Bitrate { get; } = bitrate;

        public int? UserLimit { get; } = userLimit;

        public string? RtcRegion { get; } = rtcRegion;

        public int? VideoQualityMode { get; } = videoQualityMode;
    }
}
