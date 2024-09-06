using SimpleDiscord.Components;

namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class EmojisUpdateDataMember(long guildId, Emoji[] emojis)
    {
        public long GuildId { get; } = guildId;

        public Emoji[] Emojis { get; set; } = emojis;
    }
}
