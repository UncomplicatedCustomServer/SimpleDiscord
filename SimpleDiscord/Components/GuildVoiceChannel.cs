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
