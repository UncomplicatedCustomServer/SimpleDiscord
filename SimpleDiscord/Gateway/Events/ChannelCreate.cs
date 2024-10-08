﻿using Newtonsoft.Json.Linq;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    [InternalEvent("CHANNEL_CREATE")]
    internal class ChannelCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg), IUserDeniableEvent
    {
        public GuildChannel Data { get; private set; }

        public GuildChannel Channel => Data;

        public bool CanShare { get; set; } = true;

        public override void Init()
        {
            JObject obj = JObject.Parse(RawData);

            SocketChannel raw = obj.ToObject<SocketChannel>();

            if (raw.Type is 1)
            {
                CanShare = false;
                return;
            }

            Data = GuildChannel.Caster(obj);

            Channel.Guild.SafeUpdateChannel(Channel);

            CanShare = Data is not null;
        }
    }
}
