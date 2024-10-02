using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;
using System;
using System.Linq;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("MESSAGE_CREATE")]
    public class MessageCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg), IUserDeniableEvent
    {
        public SocketMessage Data { get; internal set; }

        public Message Message { get; internal set; }

        public bool CanShare { get; set; } = true;

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketMessage>(RawData);

            if (GuildChannel.List.FirstOrDefault(c => c.Id == Data.ChannelId) is GuildTextChannel textChannel)
                Message = new(Data, textChannel);
            else
                CanShare = false;
        }
    }
}