using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_EMOJIS_UPDATE")]
    public class EmojisUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public EmojisUpdateDataMember Data { get; private set; }

        public ResolvedEmojisUpdateDataMember Emojis { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<EmojisUpdateDataMember>(RawData);
            Emojis = new(Data.GuildId, Data.Emojis, true);
        }
    }
}
