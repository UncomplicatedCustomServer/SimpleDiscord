using SimpleDiscord.Gateway.Events.Attributes;
using SimpleDiscord.Gateway.Events.LocalizedData;
using SimpleDiscord.Gateway.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleDiscord.Gateway.Events
{
    public class BaseGatewayEvent : IGatewayEvent
    {
        public static int OpCode { get; } = -1;

        private static readonly List<EventIntents> _events = [];

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

        internal static void InitEvents()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                object[] attribs = type.GetCustomAttributes(typeof(InternalEvent), false);
                if (attribs is not null && attribs.Length > 0)
                {
                    InternalEvent attribute = (InternalEvent)attribs[0];
                    _events.Add(new(type, attribute.OpCode, attribute.Event));
                }
            }
        }

        internal static BaseGatewayEvent Parse(DiscordGatewayMessage message)
        {
            EventIntents instance = _events.FirstOrDefault(x => x.OpCode == message.OpCode && x.Event == message.EventName);

            if (instance is null)
                return null;

            BaseGatewayEvent ev = (BaseGatewayEvent)Activator.CreateInstance(instance.Type, [message]);
            ev.Init();
            return ev;
        }
    }
}