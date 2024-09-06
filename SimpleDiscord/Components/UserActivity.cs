using SimpleDiscord.Enums;

namespace SimpleDiscord.Components
{
#nullable enable
    public class UserActivity(string name, int type, string? url, int createdAt, long? applicationId, string? details, string? state, Emoji? emoji, bool? instance, int? flags, ActivityButton[]? buttons)
    {
        public string Name { get; } = name;

        public ActivityType Type { get; } = (ActivityType)type;

        public string? Url { get; } = url;

        public int CreatedAt { get; } = createdAt;

        public long? ApplicationId { get; } = applicationId;

        public string? Details { get; } = details;

        public string? State { get; } = state;

        public Emoji? Emoji { get; } = emoji;

        public bool? Instance { get; } = instance;

        public int? Flags { get; } = flags;

        public ActivityButton[]? Buttons { get; } = buttons;
    }
}
