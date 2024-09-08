using SimpleDiscord.Components;
using SimpleDiscord.Enums;
using System;
using System.Linq;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
#pragma warning disable CS8618
#nullable enable
    public class ResolvedMessageAddReactionDataMember : MessageAddReactionDataMember
    {
        public SocketUser User { get; }

        public GuildTextChannel Channel { get; }

        public Message? Message { get; } // not populated if Config.SaveMessages is false

        public Guild Guild { get; }

        public new Member Member { get; }

        public new Emoji Emoji { get; }

        public SocketUser MessageAuthor { get; }

        public new ReactionType Type { get; }

        public Reaction Reaction { get; }

        public ResolvedMessageAddReactionDataMember(MessageAddReactionDataMember instance) : base(instance.UserId, instance.ChannelId, instance.MessageId, instance.GuildId, instance.Member, instance.Emoji, instance.MessageAuthorId, instance.Burst, instance.BurstColors, instance.Type)
        {
            if (instance.GuildId is null or 0)
                throw new Exception("Guild cannot be null!\nFound " + instance.GuildId);

            User = SocketUser.List.FirstOrDefault(u => u.Id == UserId);

#pragma warning disable CS8629
            Guild = Guild.GetSafeGuild((long)GuildId);
#pragma warning restore CS8629

            if (Guild.GetSafeChannel(ChannelId) is GuildTextChannel textChannel)
                Channel = textChannel;

            Message = Channel?.Messages.FirstOrDefault(m => m.Id == MessageId);
            Emoji = Guild.Emojis.FirstOrDefault(e => e.Encode() == instance.Emoji.Encode());
            MessageAuthor = SocketUser.List.FirstOrDefault(u => u.Id == MessageAuthorId);

            Type = (ReactionType)instance.Type;
        }
    }
}
