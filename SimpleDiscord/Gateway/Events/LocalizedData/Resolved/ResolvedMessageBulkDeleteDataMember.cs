using SimpleDiscord.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
#pragma warning disable CS8618
#nullable enable
    public class ResolvedMessageBulkDeleteDataMember : MessageBulkDeleteDataMember
    {
        public HashSet<Message?> Messages { get; } = [];

        public GuildTextChannel Channel { get; }

        public Guild? Guild { get; }

        public ResolvedMessageBulkDeleteDataMember(MessageBulkDeleteDataMember member) : base(member.Ids, member.ChannelId, member.GuildId)
        {
            if (GuildId is null or 0)
                return; // Cannot handle DMs

            Guild = Guild.GetGuild((long)GuildId);

            if (Guild?.GetSafeChannel(ChannelId) is GuildTextChannel textChannel)
                Channel = textChannel;
            else
                throw new Exception("Channel cannot be null!");

            foreach (long messageId in Ids)
            {
                Channel.SafeClearMessage(messageId);
                Message message = Channel.Messages.FirstOrDefault(m => m.Id == messageId);
                message?.Dispose();
                Messages.Add(message);
            }
        }
    }
}
