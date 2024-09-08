using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData.Resolved
{
    public class ResolvedEmojisUpdateDataMember : EmojisUpdateDataMember
    {
        public Guild Guild { get; }

        public ResolvedEmojisUpdateDataMember(long guildId, Emoji[] emojis, bool forcePush = false) : base(guildId, emojis)
        {
            Guild = Guild.GetGuild(guildId);

            if (forcePush)
                Guild.SafeBulkUpdateEmojis(emojis);
        }
    }
}
