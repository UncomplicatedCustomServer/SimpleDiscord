using SimpleDiscord.Components;
using System;
using System.Linq;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
#pragma warning disable CS8618 // Null thing
#nullable enable
    public class ResolvedMessageDeleteDataMember : MessageDeleteDataMember
    {
        public Message? Message { get; } // Populated only if Config.SaveMessages is true!

        public GuildTextChannel? Channel { get; }

        public Guild? Guild { get; }

        public ResolvedMessageDeleteDataMember(MessageDeleteDataMember member) : base(member.Id, member.ChannelId, member.GuildId)
        {
            if (GuildId is null or 0)
                return; // Cannot handle DMs

            Guild = Guild.GetGuild((long)GuildId);

            if (Guild?.GetSafeChannel(ChannelId) is GuildTextChannel textChannel)
                Channel = textChannel;
            else
                throw new Exception("Channel cannot be null!");

            Message = Channel.Messages.FirstOrDefault(m => m.Id == Id);
            Message?.Dispose();
            Channel.SafeClearMessage(Id);
        }
    }
}
