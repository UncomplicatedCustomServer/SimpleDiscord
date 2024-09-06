using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketMember
    {
        public SocketUser? User { get; internal set; }

        public string? Nick { get; }

        public string? Avatar { get; }

        public long[] Roles { get; }

        public string JoinedAt { get; }

        public string? PremiumSince { get; }

        public bool Deaf { get; }

        public bool Mute { get; }

        public int Flags { get; }

        public bool? Pending { get; }

        public string? Permissions { get; }

        public string? CommunicationDisabledUntil { get; }

        [JsonConstructor]
        public SocketMember(SocketUser? user, string? nick, string? avatar, long[] roles, string joinedAt, string? premiumSince, bool deaf, bool mute, int flags, bool? pending, string? permissions, string? communicationDisabledUntil)
        {
            User = user;
            Nick = nick;
            Avatar = avatar;
            Roles = roles;
            JoinedAt = joinedAt;
            PremiumSince = premiumSince;
            Deaf = deaf;
            Mute = mute;
            Flags = flags;
            Pending = pending;
            Permissions = permissions;
            CommunicationDisabledUntil = communicationDisabledUntil;
        }

        public SocketMember(SocketMember self)
        {
            User = self.User;
            Nick = self.Nick;
            Avatar = self.Avatar;
            Roles = self.Roles;
            JoinedAt = self.JoinedAt;
            PremiumSince = self.PremiumSince;
            Deaf = self.Deaf;
            Mute = self.Mute;
            Flags = self.Flags;
            Pending = Pending;
            Permissions = self.Permissions;
            CommunicationDisabledUntil = self.CommunicationDisabledUntil;
        }
    }
}
