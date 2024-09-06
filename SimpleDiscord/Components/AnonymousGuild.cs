using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SimpleDiscord.Components
{
    public class AnonymousGuild : SocketGuild
    {
        [JsonProperty("joined_at")]
        public string JoinedAt { get; }

        public bool Large { get; }

        public int MemberCount { get; }

        public SocketMember[] Members { get; }

        public SocketGuildChannel[] Channels { get; }

        public SocketGuildChannel[] Threads { get; }

        public SocketSendPresence[] Presences { get; }

        public SocketStageInstance[] StageInstances { get; }

        public SocketScheduledEvent[] GuildScheduledEvents { get; }

        [JsonConstructor]
        public AnonymousGuild(string joinedAt, bool large, int memberCount, SocketMember[] members, SocketGuildChannel[] channels, SocketGuildChannel[] threads, SocketSendPresence[] presences, SocketStageInstance[] stageInstances, SocketScheduledEvent[] guildScheduledEvents, long id, string name, string icon, string splash, string discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string vanityUrlCode, string description, string banner, long? publicUpdatedChannelId, int nsfwLevel, bool premiumProgressBarEnable) : base(id, name, icon, splash, discoverySplash, afkChannelId, afkTimeout, verificationLevel, defaultMessageNotifications, explicitContentFilter, roles, emojis, features, mfaLevel, premiumTier, preferredLocale, applicationId, systemChannelId, systemChannelFlags, rulesChannelId, maxPresence, maxMembers, vanityUrlCode, description, banner, publicUpdatedChannelId, nsfwLevel, premiumProgressBarEnable)
        {
            JoinedAt = joinedAt;
            Large = large;
            MemberCount = memberCount;
            Members = members;

            Console.WriteLine($"\n\n{Id} - {Name}\n\n");

            Channels = channels;

            Threads = threads;
            Presences = presences;
            StageInstances = stageInstances;
            GuildScheduledEvents = guildScheduledEvents;

            new Guild(this);
        }

        public AnonymousGuild(SocketGuild socket) : base(socket)
        { }
    }
}
