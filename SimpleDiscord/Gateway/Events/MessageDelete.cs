using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_DELETE")]
    public class MessageDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MessageDeleteDataMember Data { get; private set; }

        public ResolvedMessageDeleteDataMember Message { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MessageDeleteDataMember>(RawData);
            Message = new(Data);
        }
    }
}
