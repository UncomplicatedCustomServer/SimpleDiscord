using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_REACTION_ADD")]
    public class ReactionAdd(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public MessageAddReactionDataMember Data { get; private set; }

        public ResolvedMessageAddReactionDataMember Reaction { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<MessageAddReactionDataMember>(RawData);
            Reaction = new(Data);
        }
    }
}
