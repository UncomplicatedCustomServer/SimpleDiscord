using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;

namespace SimpleDiscord.Components
{
#nullable enable
    [SocketInstance(typeof(SocketGuildVoiceChannel))]
    public class GuildVoiceChannel : GuildTextChannel, IGuildElement
    {
        public int Bitrate { get; }

        public int UserLimit { get; }

        public string? RtcRegion { get; }

        public int? VideoQualityMode { get; }

        [JsonConstructor]
        public GuildVoiceChannel(long id, int type, long guildId, int? position, Overwrite[]? permissionOverwrites, string? name, long? parentId, string? permissions, int? flags, string? topic, bool? nsfw, long? lastMessageId, int? rateLimitPerUser, int? totalMessageSent, int bitrate, int userLimit, string? rtcRegion, int? videoQualityMode, bool safeUpdate = true) : base(id, type, guildId, position, permissionOverwrites, name, parentId, permissions, flags, topic, nsfw, lastMessageId, rateLimitPerUser, totalMessageSent, false)
        {
            Bitrate = bitrate;
            UserLimit = userLimit;
            RtcRegion = rtcRegion;
            VideoQualityMode = videoQualityMode;

            if (safeUpdate)
                Guild.SafeUpdateChannel(this);
        }

        public GuildVoiceChannel(GuildTextChannel baseChannel, int bitrate, int userLimit, string? rtcRegion, int? videoQualityMode) : base(baseChannel)
        {
            Bitrate = bitrate;
            UserLimit = userLimit;
            RtcRegion = rtcRegion;
            VideoQualityMode = videoQualityMode;
        }

        public GuildVoiceChannel(GuildVoiceChannel baseChannel) : this(baseChannel, baseChannel.Bitrate, baseChannel.UserLimit, baseChannel.RtcRegion, baseChannel.VideoQualityMode) 
        { }

        public GuildVoiceChannel(SocketGuildVoiceChannel socketChannel, bool pushUpdate = false) : base(socketChannel)
        {
            Bitrate = socketChannel.Bitrate;
            UserLimit = socketChannel.UserLimit;
            RtcRegion = socketChannel.RtcRegion;
            VideoQualityMode = socketChannel.VideoQualityMode;

            if (pushUpdate)
                Guild.SafeUpdateChannel(this);
        }
    }
}
