using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_MEMBERS_CHUNK")]
    public class GuildMemberChunk(DiscordGatewayMessage msg) : BaseGatewayEvent(msg), IUserDeniableEvent
    {
        public bool CanShare { get; set; } = false;

        public GuildMemberChunkMemberData Data { get; private set; }

        public Guild Guild { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<GuildMemberChunkMemberData>(RawData);
            Guild = Guild.GetSafeGuild(Data.GuildId);

            foreach (SocketMember member in Data.Members)
                Guild.SafeUpdateMember(new(Guild, member));
        }
    }
}
