using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_DELETE_BULK")]
    public class MessageBulkDelete(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MessageBulkDeleteDataMember Data { get; private set; }

        public ResolvedMessageBulkDeleteDataMember Messages { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MessageBulkDeleteDataMember>(RawData);
            Messages = new(Data);
        }
    }
}
