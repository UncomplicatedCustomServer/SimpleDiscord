using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class ResolvedData : SocketResolvedData
    {
        public new Dictionary<long, GuildChannel> Channels { get; } = [];

        public new Dictionary<long, Message> Messages { get; } = [];

        public ResolvedData(Dictionary<long, SocketUser>? users, Dictionary<long, SocketMember>? members, Dictionary<long, Role>? roles, Dictionary<long, object>? channels, Dictionary<long, SocketMessage>? messages, Dictionary<long, Attachment>? attachments) : base(users, members, roles, channels, messages, attachments)
        {
            if (channels is not null)
                foreach (KeyValuePair<long, object> channel in channels)
                    if (channel.Value is JObject obj)
#nullable disable
                        Channels.Add(channel.Key, GuildChannel.Caster(obj));
#nullable enable

            if (messages is not null)
                foreach (KeyValuePair<long, SocketMessage> message in messages)
                    Messages.Add(message.Key, new(message.Value));
        }

        public ResolvedData(SocketResolvedData resolvedData) : this(resolvedData.Users, resolvedData.Members, resolvedData.Roles, resolvedData.Channels, resolvedData.Messages, resolvedData.Attachments)
        { }
    }
}
