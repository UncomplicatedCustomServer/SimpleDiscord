using SimpleDiscord.Gateway.Events.LocalizedData;
using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
    public class Guild : SocketGuild
    {
        public new static HashSet<Guild> List { get; } = [];

        public string JoinedAt { get; }

        public bool Large { get; }

        public int MemberCount { get; }

        public Member[] Members { get; }

        public SocketGuildTextChannel[] Channels { get; }

        public SocketGuildTextChannel[] Threads { get; }

        public SocketPresence[] Presences { get; }

        public SocketStageInstance[] StageInstances { get; }

        public SocketScheduledEvent[] GuildScheduledEvents { get; }

        public Guild(string joinedAt, bool large, int memberCount, Member[] members, SocketGuildTextChannel[] channels, SocketGuildTextChannel[] threads, SocketPresence[] presences, SocketStageInstance[] stageInstances, SocketScheduledEvent[] guildScheduledEvents, long id, string name, string icon, string splash, string discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string vanityUrlCode, string description, string banner, long? publicUpdatedChannelId, int nsfwLevel, bool premiumProgressBarEnable) : base(id, name, icon, splash, discoverySplash, afkChannelId, afkTimeout, verificationLevel, defaultMessageNotifications, explicitContentFilter, roles, emojis, features, mfaLevel, premiumTier, preferredLocale, applicationId, systemChannelId, systemChannelFlags, rulesChannelId, maxPresence, maxMembers, vanityUrlCode, description, banner, publicUpdatedChannelId, nsfwLevel, premiumProgressBarEnable)
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

            if (!List.Any(guild => guild.Id == id))
                List.Add(this);
        }

        public Guild(long id, string name, string icon, string splash, string discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string vanityUrlCode, string description, string banner, long? publicUpdatedChannelId, int nsfwLevel, bool premiumProgressBarEnable) : base(id, name, icon, splash, discoverySplash, afkChannelId, afkTimeout, verificationLevel, defaultMessageNotifications, explicitContentFilter, roles, emojis, features, mfaLevel, premiumTier, preferredLocale, applicationId, systemChannelId, systemChannelFlags, rulesChannelId, maxPresence, maxMembers, vanityUrlCode, description, banner, publicUpdatedChannelId, nsfwLevel, premiumProgressBarEnable)
        {
            if (!List.Any(guild => guild.Id == id))
                List.Add(this);
        }

        public Guild(SocketGuild socket) : this(socket.Id, socket.Name, socket.Icon, socket.Splash, socket.DiscoverySplash, socket.AfkChannelId, socket.AfkTimeout, socket.VerificationLevel, socket.DefaultMessageNotifications, socket.ExplicitContentFilter, socket.Roles, socket.Emojis, socket.Features, socket.MfaLevel, socket.PremiumTier, socket.PreferredLocale, socket.ApplicationId, socket.SystemChannelId, socket.SystemChannelFlags, socket.RulesChannelId, socket.MaxPresence, socket.MaxMembers, socket.VanityUrlCode, socket.Description, socket.Banner, socket.PublicUpdatedChannelId, socket.NsfwLevel, socket.PremiumProgressBarEnable)
        { }

        public Guild(GuildCreatedMember member) : this(member as SocketGuild)
        { }
    }
}
