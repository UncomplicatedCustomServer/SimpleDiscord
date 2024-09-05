using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#pragma warning disable IDE1006
#nullable enable
    internal class SocketReaction : SocketMessageObject
    {
        public SocketMember? Member { get; }

        public Emoji Emoji { get; }

        public bool Burst { get; }

        public string[]? BurstColors { get; }

        public int Type { get; }

        [JsonIgnore]
        public bool Complete => Member is not null;

        [JsonConstructor]
        public SocketReaction(long userId, long channelId, long guildId, SocketMember? member, Emoji emoji, bool burst, string[]? burstColors, int type) : base(userId, channelId, guildId)
        {
            Member = member;
            Emoji = emoji;
            Burst = burst;
            BurstColors = burstColors;
            Type = type;
        }

        public SocketReaction(SocketMessageObject obj, SocketMember? member, Emoji emoji, bool burst, string[]? burstColors, int type) : base(obj)
        {
            Member = member;
            Emoji = emoji;
            Burst = burst;
            BurstColors = burstColors;
            Type = type;
        }

        public SocketReaction(SocketReaction self) : this(self, self.Member, self.Emoji, self.Burst, self.BurstColors, self.Type)
        { }
    }
}
