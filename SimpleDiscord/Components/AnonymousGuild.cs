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

        [JsonProperty("member_count")]
        public int MemberCount { get; }

        public SocketMember[] Members { get; }

        public SocketGuildChannel[] Channels { get; }

        public SocketGuildChannel[] Threads { get; }

        public SocketPresence[] Presences { get; }

        [JsonProperty("stage_instances")]
        public SocketStageInstance[] StageInstances { get; }

        [JsonProperty("guild_scheduled_events")]
        public SocketScheduledEvent[] GuildScheduledEvents { get; }

        [JsonConstructor]
        public AnonymousGuild(string joinedAt, bool large, int memberCount, SocketMember[] members, SocketGuildChannel[] channels, SocketGuildChannel[] threads, SocketPresence[] presences, SocketStageInstance[] stageInstances, SocketScheduledEvent[] guildScheduledEvents, long id, string name, string icon, string splash, string discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string vanityUrlCode, string description, string banner, long? publicUpdatedChannelId, int nsfwLevel, bool premiumProgressBarEnable) : base(id, name, icon, splash, discoverySplash, afkChannelId, afkTimeout, verificationLevel, defaultMessageNotifications, explicitContentFilter, roles, emojis, features, mfaLevel, premiumTier, preferredLocale, applicationId, systemChannelId, systemChannelFlags, rulesChannelId, maxPresence, maxMembers, vanityUrlCode, description, banner, publicUpdatedChannelId, nsfwLevel, premiumProgressBarEnable)
        {
            JoinedAt = joinedAt;
            Large = large;
            MemberCount = memberCount;
            Members = members;

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
