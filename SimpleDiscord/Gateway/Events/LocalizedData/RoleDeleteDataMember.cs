namespace SimpleDiscord.Gateway.Events.LocalizedData
{
    public class RoleDeleteDataMember(long guildId, long roleId)
    {
        public long GuildId { get; } = guildId;

        public long RoleId { get; } = roleId;
    }
}
