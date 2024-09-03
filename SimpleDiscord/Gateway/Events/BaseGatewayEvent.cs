using SimpleDiscord.Gateway.Messages;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleDiscord.Gateway.Events
{
    internal class BaseGatewayEvent(DiscordGatewayMessage msg) : IGatewayEvent
    {
        public static int OpCode { get; } = -1;

        private static readonly List<Type> _events = [typeof(Hello), typeof(HeartbeatAck), typeof(Ready), typeof(InvalidSession), typeof(GuildCreate), typeof(MessageCreate)];

        public int InternalOpCode { get; } = msg.OpCode;

        public string RawData { get; } = msg.Data?.ToString();

        public DiscordGatewayMessage GatewayMessage { get; } = msg;

        public virtual void Init() 
        { }

        public static BaseGatewayEvent Parse(DiscordGatewayMessage message)
        {
            foreach (Type type in _events)
            {
                if (message.OpCode != 0)
                {
                    if ((int)type.GetProperty(nameof(OpCode), BindingFlags.Public | BindingFlags.Static)?.GetValue(null, null) == message.OpCode)
                    {
                        BaseGatewayEvent ev = (BaseGatewayEvent)Activator.CreateInstance(type, [message]);
                        ev.Init();
                        return ev;
                    }
                }
                else if (message.OpCode is 0 && type.GetProperty("Event", BindingFlags.Public | BindingFlags.Static) is not null && (string)type.GetProperty("Event", BindingFlags.Public | BindingFlags.Static)?.GetValue(null, null) == message.EventName.ToUpper() && message.EventName != "")
                {
                    BaseGatewayEvent ev = (BaseGatewayEvent)Activator.CreateInstance(type, [message]);
                    ev.Init();
                    return ev;
                }
            }

            return null;
        }
    }
}
