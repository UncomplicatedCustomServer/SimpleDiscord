using SimpleDiscord.Enums;
using System;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Activity(string name, int type, string? url)
    {
        public string Name { get; } = name;

        public int Type { get; } = type;

        public string? Url { get; } = url;

        public long CreatedAt { get; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public Activity(string name, ActivityType type, string? url = null) : this(name, (int)type, url)
        { }
    }
}
