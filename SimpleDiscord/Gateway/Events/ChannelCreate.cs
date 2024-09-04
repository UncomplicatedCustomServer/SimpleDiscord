using Newtonsoft.Json;
using SimpleDiscord.Components;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDiscord.Gateway.Events
{
    internal class ChannelCreate(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public new static int OpCode => 0;

        public static string Event => "CHANNEL_CREATE";

        public SocketGenericChannel Data { get; private set; }

        public override void Init()
        {
            Data = JsonConvert.DeserializeObject<SocketGenericChannel>(RawData);
        }
    }
}
