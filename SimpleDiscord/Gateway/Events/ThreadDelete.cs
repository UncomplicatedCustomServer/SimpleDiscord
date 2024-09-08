using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("THREAD_DELETE")]
    public class ThreadDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketGuildChannel Data { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketGuildChannel>(RawData);
            if (Data.GuildId is 0)
                return;

            Guild.GetSafeGuild(Data.GuildId).SafeClearThread(Data.Id);
        }
    }
}