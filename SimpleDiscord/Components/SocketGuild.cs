using Newtonsoft.Json;
using SimpleDiscord.Components.Attributes;

namespace SimpleDiscord.Components
{
#nullable enable
    [EndpointInfo("/guilds/{guild.id}", "GUILD")]
    public class SocketGuild : ClientChild
    {
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

        [JsonConstructor]
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
        }

        public SocketGuild(SocketGuild guild)
        {
            Id = guild.Id;
            Name = guild.Name;
            Icon = guild.Icon;
            Splash = guild.Splash;
            DiscoverySplash = guild.DiscoverySplash;
            AfkChannelId = guild.AfkChannelId;
            AfkTimeout = guild.AfkTimeout;
            VerificationLevel = guild.VerificationLevel;
            DefaultMessageNotifications = guild.VerificationLevel;
            ExplicitContentFilter = guild.ExplicitContentFilter;
            Roles = guild.Roles;
            Emojis = guild.Emojis;
            Features = guild.Features;
            MfaLevel = guild.MfaLevel;
            PremiumTier = guild.PremiumTier;
            PreferredLocale = guild.PreferredLocale;
            ApplicationId = guild.ApplicationId;
            SystemChannelId = guild.SystemChannelId;
            SystemChannelFlags = guild.SystemChannelFlags;
            RulesChannelId = guild.RulesChannelId;
            MaxPresence = guild.MaxPresence;
            MaxMembers = guild.MaxMembers;
            VanityUrlCode = guild.VanityUrlCode;
            Description = guild.Description;
            Banner = guild.Banner;
            PublicUpdatedChannelId = guild.PublicUpdatedChannelId;
            NsfwLevel = guild.NsfwLevel;
            PremiumProgressBarEnable = guild.PremiumProgressBarEnable;
        }
    }
}
