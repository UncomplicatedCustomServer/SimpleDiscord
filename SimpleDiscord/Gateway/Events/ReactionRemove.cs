using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_REACTION_REMOVE")]
    public class ReactionRemove(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MessageRemoveReactionDataMember Data { get; private set; }

        public ResolvedMessageRemoveReactionDataMember Reaction { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MessageRemoveReactionDataMember>(RawData);
            Reaction = new(Data);
        }
    }
}
