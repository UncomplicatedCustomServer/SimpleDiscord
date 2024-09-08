using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Events.LocalizedData.Resolved;
using SimpleDiscord.Gateway.Messages;
using System.Linq;

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

            if (GuildChannel.List.FirstOrDefault(c => c.Id == Data.ChannelId) is GuildTextChannel textChannel)
                Message = new(Data);
        }
    }
}
