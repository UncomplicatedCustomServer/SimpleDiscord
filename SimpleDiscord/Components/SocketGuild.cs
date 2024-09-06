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

        [JsonProperty("discovery_splash")]
        public string? DiscoverySplash { get; }

        [JsonProperty("afk_channel_id")]
        public long? AfkChannelId { get; }

        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; }

        [JsonProperty("verification_level")]
        public int VerificationLevel { get; }

        [JsonProperty("default_message_notifications")]
        public int DefaultMessageNotifications { get; }

        [JsonProperty("explicit_content_filter")]
        public int ExplicitContentFilter { get; }

        public Role[] Roles { get; }

        public Emoji[] Emojis { get; } 

        public string[] Features { get; }

        [JsonProperty("mfa_level")]
        public int MfaLevel { get; }

        [JsonProperty("premium_tier")]
        public int PremiumTier { get; }

        [JsonProperty("preferred_locale")]
        public string PreferredLocale { get; }

        [JsonProperty("application_id")]
        public long? ApplicationId { get; }

        [JsonProperty("system_channel_id")]
        public long? SystemChannelId { get; }

        [JsonProperty("system_channel_flags")]
        public int SystemChannelFlags { get; }

        [JsonProperty("rules_channel_id")]
        public long? RulesChannelId { get; }

        [JsonProperty("max_presence")]
        public int? MaxPresence { get; }

        [JsonProperty("max_members")]
        public int MaxMembers { get; }

        [JsonProperty("vanity_url_code")]
        public string? VanityUrlCode { get; }

        public string? Description { get; }

        public string? Banner { get; }

        [JsonProperty("public_updates_channel_id")]
        public long? PublicUpdatesChannelId { get; }

        [JsonProperty("nsfw_level")]
        public int NsfwLevel { get; }

        [JsonProperty("premium_progress_bar_enable")]
        public bool PremiumProgressBarEnable { get; }

        [JsonConstructor]
        public SocketGuild(long id, string name, string? icon, string? splash, string? discoverySplash, long? afkChannelId, int afkTimeout, int verificationLevel, int defaultMessageNotifications, int explicitContentFilter, Role[] roles, Emoji[] emojis, string[] features, int mfaLevel, int premiumTier, string preferredLocale, long? applicationId, long? systemChannelId, int systemChannelFlags, long? rulesChannelId, int? maxPresence, int maxMembers, string? vanityUrlCode, string? description, string? banner, long? publicUpdatesChannelId, int nsfwLevel, bool premiumProgressBarEnable)
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
            PublicUpdatesChannelId = publicUpdatesChannelId;
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
            PublicUpdatesChannelId = guild.PublicUpdatesChannelId;
            NsfwLevel = guild.NsfwLevel;
            PremiumProgressBarEnable = guild.PremiumProgressBarEnable;
        }
    }
}
