using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using System;
using System.Linq;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedMessageRemoveReactionDataMember : MessageRemoveReactionDataMember
    {
        public SocketUser User { get; }

        public GuildTextChannel Channel { get; }

        public Message Message { get; }

        public Guild Guild { get; }

        public new Emoji Emoji { get; }

        public new ReactionType Type { get; }

        public ResolvedMessageRemoveReactionDataMember(MessageRemoveReactionDataMember instance) : base(instance.UserId, instance.ChannelId, instance.MessageId, instance.GuildId, instance.Emoji, instance.Burst, instance.Type)
        {
            if (GuildId is null or 0)
                throw new Exception("Guild is null!");

            User = SocketUser.List.FirstOrDefault(u => u.Id == UserId);
            Guild = Guild.GetGuild((long)GuildId);
            if (Guild.GetSafeChannel(ChannelId) is GuildTextChannel textChannel)
                Channel = textChannel;

            Emoji = Guild.Emojis.FirstOrDefault(e => e.Encode() == instance.Emoji.Encode());
            Message = Channel.Messages.FirstOrDefault(msg => msg.Id == MessageId);

            Type = (ReactionType)instance.Type;
        }
    }
}
