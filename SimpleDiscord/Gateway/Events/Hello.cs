using Newtonsoft.Json;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;
using System;

namespace SimpleDiscord.Gateway.Events
{
    internal class Hello(DiscordGatewayMessage msg) : BaseGatewayEvent(msg)
    {
        public static new int OpCode => 10;

        public HelloDataMember Data { get; private set; }

        public override void Init()
        {
            Console.WriteLine(RawData);
            try
            {
                Data = JsonConvert.DeserializeObject<HelloDataMember>(RawData);
                Console.WriteLine($"\nACTED: {Data.HeartbeatInterval}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
