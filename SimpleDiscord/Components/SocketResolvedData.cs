using System.Collections.Generic;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketResolvedData(Dictionary<long, SocketUser>? users, Dictionary<long, SocketMember>? members, Dictionary<long, Role>? roles, Dictionary<long, object>? channels, Dictionary<long, SocketMessage>? messages, Dictionary<long, Attachment>? attachments)
    {
        public Dictionary<long, SocketUser>? Users { get; } = users;

        public Dictionary<long, SocketMember>? Members { get; } = members;

        public Dictionary<long, Role>? Roles { get; } = roles;

        public Dictionary<long, object>? Channels { get; } = channels;

        public Dictionary<long, SocketMessage>? Messages { get; } = messages;

        public Dictionary<long, Attachment>? Attachments { get; } = attachments;
    }
}
