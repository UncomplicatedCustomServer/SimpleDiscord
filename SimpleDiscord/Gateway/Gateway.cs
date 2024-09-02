using SimpleDiscord.Enums;

namespace SimpleDiscord.Gateway
{
    public class Gateway
    {
        public const GatewayIntents defaultIntents = GatewayIntents.GUILDS | GatewayIntents.GUILD_MODERATION | GatewayIntents.GUILD_EMOJIS_AND_STICKERS | GatewayIntents.GUILD_INTEGRATIONS | GatewayIntents.GUILD_WEBHOOKS | GatewayIntents.GUILD_INVITES | GatewayIntents.GUILD_VOICE_STATES | GatewayIntents.GUILD_MESSAGES | GatewayIntents.GUILD_MESSAGE_REACTIONS | GatewayIntents.GUILD_MESSAGE_TYPING | GatewayIntents.DIRECT_MESSAGES | GatewayIntents.DIRECT_MESSAGE_REACTIONS | GatewayIntents.DIRECT_MESSAGE_TYPING | GatewayIntents.GUILD_SCHEDULED_EVENTS | GatewayIntents.AUTO_MODERATION_CONFIGURATION | GatewayIntents.AUTO_MODERATION_EXECUTION | GatewayIntents.GUILD_MESSAGE_POOLS;

        public const GatewayIntents privilegedIntents = GatewayIntents.GUILD_MEMBERS | GatewayIntents.GUILD_PRESENCES | GatewayIntents.MESSAGE_CONTENT;

        public const GatewayIntents all = defaultIntents | privilegedIntents;
    }
}
