using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketGuild
    {
        public static HashSet<SocketGuild> List { get; } = [];

        public long Id { get; }

        public string Name { get; }

        public string? Icon { get; } 

        public string? Splash { get; }

        public string? DiscoverySplash { get; } 

        public long? AfkChannelId { get; }

        public int AfkTimeout { get; }

        public int VerificationLevel { get; }

        public int DefaultMessageNotifications { get; }

        public int ExplicitContentFilter { get; }

        public Role[] Roles { get; }

        public Emoji[] Emojis { get; } 

        public string[] Features { get; }

        public int MfaLevel { get; }

        public int PremiumTier { get; }

        public string PreferredLocale { get; }

        public long? ApplicationId { get; }

        public long? SystemChannelId { get; }

        public int SystemChannelFlags { get; }

        public long? RulesChannelId { get; }

        public int? MaxPresence { get; }

        public int MaxMembers { get; } 

        public string? VanityUrlCode { get; }

        public string? Description { get; }

        public string? Banner { get; }

        public long? PublicUpdatedChannelId { get; }

        public int NsfwLevel { get; }

        public bool PremiumProgressBarEnable { get; }

        public SocketGuild(long id, string name, string? icon, string? splash, string? discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string? vanityUrlCode, string? description, string? banner, long? publicUpdatedChannelId, int nsfwLevel, bool premiumProgressBarEnable)
        {
            Id = id;
            Name = name;
            Icon = icon;
            Splash = splash;
            DiscoverySplash = discoverySplash;
            AfkChannelId = afkChannelId;
            AfkTimeout = afkTimeout;
            VerificationLevel = verificationLevel;
            DefaultMessageNotifications = defaultMessageNotifications;
            ExplicitContentFilter = explicitContentFilter;
            Roles = roles;
            Emojis = emojis;
            Features = features;
            MfaLevel = mfaLevel;
            PremiumTier = premiumTier;
            PreferredLocale = preferredLocale;
            ApplicationId = applicationId;
            SystemChannelId = systemChannelId;
            SystemChannelFlags = systemChannelFlags;
            RulesChannelId = rulesChannelId;
            MaxPresence = maxPresence;
            MaxMembers = maxMembers;
            VanityUrlCode = vanityUrlCode;
            Description = description;
            Banner = banner;
            PublicUpdatedChannelId = publicUpdatedChannelId;
            NsfwLevel = nsfwLevel;
            PremiumProgressBarEnable = premiumProgressBarEnable;

            if (!List.Any(guild => guild.Id == Id))
                List.Add(this);
        }
    }
}
