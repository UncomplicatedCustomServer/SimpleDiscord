namespace SimpleDiscord.Components
{
    public class MessageObject : SocketMessageObject
    {
        public Message Message { get; }

        public GuildTextChannel Channel { get; }

        public Guild Guild { get; }

        public MessageObject(SocketMessageObject baseElement) : base(baseElement)
        {
            if (GuildId is null)
                return;

            Guild = Guild.GetSafeGuild((long)GuildId);
            Channel = Guild.GetSafeChannel(ChannelId) as GuildTextChannel;
            Message = Channel.GetSafeMessage(MessageId);
        }
    }
}
