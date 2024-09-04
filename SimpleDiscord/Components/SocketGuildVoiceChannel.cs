using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuildVoiceChannel : SocketGuildTextChannel
    {
        public int Bitrate { get; }

        public int UserLimit { get; }

        public string? RtcRegion { get; }

        public int? VideoQualityMode { get; }

        [JsonConstructor]
        public SocketGuildVoiceChannel(long id, int type, long guildId, int? position, Overwrite[] permissionOverwrites, string name, string topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, long? parentId, string permissions, int? flags, int? totalMessageSent, int bitrate, int userLimit, string? rtcRegion, int? videoQualityMode) : base(id, type, guildId, position, permissionOverwrites, name, topic, nsfw, lastMessageId, rateLimitPerUser, parentId, permissions, flags, totalMessageSent)
        {
            Bitrate = bitrate;
            UserLimit = userLimit;
            RtcRegion = rtcRegion;
            VideoQualityMode = videoQualityMode;
        }

        public SocketGuildVoiceChannel(SocketGuildTextChannel baseChannel, int bitrate, int userLimit, string? rtcRegion, int? videoQualityMode) : base(baseChannel)
        {
            Bitrate = bitrate;
            UserLimit = userLimit;
            RtcRegion = rtcRegion;
            VideoQualityMode = videoQualityMode;
        }

        public SocketGuildVoiceChannel(GuildVoiceChannel baseChannel) : base(baseChannel)
        {
            Bitrate = baseChannel.Bitrate;
            UserLimit = baseChannel.UserLimit;
            RtcRegion = baseChannel.RtcRegion;
            VideoQualityMode = baseChannel.VideoQualityMode;
        }
    }
}