using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class Application : PartialApplication
    {
        public string Name { get; }

        public string? Icon { get; }

        public string Description { get; }

        public bool BotPublic { get; }

        public bool BotRequireCodeGrant { get; }

        public SocketUser? Bot { get; }

        public string? TermsOfServiceUrl { get; }

        public string? PrivacyPolicyUrl { get; }

        public SocketUser? Owner { get; }

        public string VerifyKey { get; }

        public object? Team { get; }

        public long? GuildId { get; }

        public int? ApproximateGuildCount { get; }

        public string[] Tags { get; }

        [JsonConstructor]
        public Application(long id, string name, string? icon, string description, bool botPublic, bool botRequireCodeGrant, SocketUser? bot, string? termsOfServiceUrl, string? privacyPolicyUrl, SocketUser? owner, string verifyKey, object? team, long? guildId, int? flags, int? approximateGuildCount, string[] tags) : base(id, flags)
        {
            Name = name;
            Icon = icon;
            Description = description;
            BotPublic = botPublic;
            BotRequireCodeGrant = botRequireCodeGrant;
            Bot = bot;
            TermsOfServiceUrl = termsOfServiceUrl;
            PrivacyPolicyUrl = privacyPolicyUrl;
            Owner = owner;
            VerifyKey = verifyKey;
            Team = team;
            GuildId = guildId;
            ApproximateGuildCount = approximateGuildCount;
            Tags = tags;
        }

        public Application(Application self) : base(self.Id, self.Flags)
        {
            Name = self.Name;
            Icon = self.Icon;
            Description = self.Description;
            Bot = self.Bot;
            BotPublic = self.BotPublic;
            BotRequireCodeGrant = self.BotRequireCodeGrant;
            TermsOfServiceUrl = self.TermsOfServiceUrl;
            PrivacyPolicyUrl = self.PrivacyPolicyUrl;
            Owner = self.Owner;
            VerifyKey = self.VerifyKey;
            Team = self.Team;
            GuildId = self.GuildId;
            ApproximateGuildCount = self.ApproximateGuildCount;
            Tags = self.Tags;
        }
    }
}
