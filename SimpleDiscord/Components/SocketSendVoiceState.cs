namespace SimpleDiscord.Components
{
    public class SocketSendVoiceState(long? channelId, bool? suppress)
    {
        public long? ChannelId { get; } = channelId;

        public bool? Suppress { get; } = suppress;

        public static SocketSendVoiceState Move(long channelId) => new(channelId, null);

        public static SocketSendVoiceState Mute(bool suppress) => new(null, suppress);
    }
}
