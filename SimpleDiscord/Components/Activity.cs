using Newtonsoft.Json;
using SimpleDiscord.Enums;
using System;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Activity
    {
        public string Name { get; }

        public int Type { get; }

        public string? Url { get; }

        public long CreatedAt { get; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        [JsonConstructor]
        public Activity(string name, int type, string? url)
        {
            Name = name;
            Type = type;
            Url = url;
        }

        public Activity(string name, ActivityType type, string? url = null) : this(name, (int)type, url)
        { }
    }
}
