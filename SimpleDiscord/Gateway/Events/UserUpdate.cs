using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("USER_UPDATE")]
    public class UserUpdate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public SocketUser User { get; internal set; }

        public override void Init()
        {
            User = JsonConvert.DeserializeObject<SocketUser>(RawData);

            // We should update every users but nahh -- added Member::SyncUser() method
        }
    }
}
