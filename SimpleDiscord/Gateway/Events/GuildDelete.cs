using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("GUILD_DELETE")]
    public class GuildDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public UnavailableGuild Data { get; private set; }

        public UnavailableGuild Guild => Data;

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<UnavailableGuild>(RawData);

            // Dispose guild
            Guild guild = Components.Guild.GetSafeGuild(Data.Id);
            guild.Dispose();
        }
    }
}
