using SimpleDiscord.Gateway.Messages;

namespace SimpleDiscord.Gateway.Events
{
    public class BaseGatewayEvent : IGatewayEvent
    {
        public int InternalOpCode { get; }

        public string RawData { get; }

        public DiscordGatewayMessage GatewayMessage { get; }

        internal BaseGatewayEvent(DiscordGatewayMessage msg)
        {
            GatewayMessage = msg;
            RawData = msg.Data?.ToString();
            InternalOpCode = msg.OpCode;
        }

        public virtual void Init() 
        { }
    }
}