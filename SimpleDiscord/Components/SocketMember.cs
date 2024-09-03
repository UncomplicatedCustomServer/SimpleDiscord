namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketMember(SocketUser? user, string? nick, string? avatar, long[] roles, string joinedAt, string? premiumSince, bool deaf, bool mute, int flags, bool? pending, string? permissions, string? communicationDisabledUntil)
    {
        public SocketUser? User { get; } = user;

        public string? Nick { get; } = nick;

        public string? Avatar { get; } = avatar;

        public long[] Roles { get; } = roles;

        public string JoinedAt { get; } = joinedAt;

        public string? PremiumSince { get; } = premiumSince;

        public bool Deaf { get; } = deaf;

        public bool Mute { get; } = mute;

        public int Flags { get; } = flags;

        public bool? Pending { get; } = pending;

        public string? Permissions { get; } = permissions;

        public string? CommunicationDisabledUntil { get; } = communicationDisabledUntil;
    }
}
